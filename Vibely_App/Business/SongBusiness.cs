using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TagLib;
using Vibely_App.API;
using Vibely_App.Business.Helpers;
using Vibely_App.Data;
using Vibely_App.Data.Models;

namespace Vibely_App.Business
{
    public class SongBusiness(VibelyDbContext dbContext) : ISongBusiness
    {
        VibelyDbContext _dbContext = dbContext;

        public List<Song> GetAll()
        {
            return _dbContext.Songs
                .Include(s => s.Genre)
                .Include(s => s.User)
                .ToList();
        }

        public void Add(Song song)
        {
            if (_dbContext.Songs.Find(song.Id) != null)
            {
                return;
            }

            _dbContext.Songs.Add(song);
            _dbContext.SaveChanges();
        }

        public Song? Find(int id)
        {
            return _dbContext.Songs.Find(id);
        }

        public void Remove(int id)
        {
            Song? song = _dbContext.Songs.Find(id);

            if (song != null)
            {
                _dbContext.Songs.Remove(song);
                _dbContext.SaveChanges();
            }
        }

        public void Update(int id, Song song)
        {
            Song? songToUpdate = _dbContext.Songs.Find(id);

            if (songToUpdate != null)
            {
                _dbContext.Entry(songToUpdate).State = EntityState.Detached;

                songToUpdate.Title = song.Title;
                songToUpdate.Duration = song.Duration;
                songToUpdate.UserId = song.UserId;
                songToUpdate.GenreId = song.GenreId;
                songToUpdate.Data = song.Data;

                _dbContext.Attach(songToUpdate);
                _dbContext.Entry(songToUpdate).State = EntityState.Modified;

                _dbContext.SaveChanges();
            }
        }

        // Method to handle the uploaded song data
        public bool UploadSong(User user, byte[] songData, string originalFileName)
        {
            // --- Business Logic Implementation --- 

            // 1. Validate Input:
            //    - Check if songData is null or empty.
            //    - Check if originalFileName is valid.
            if (songData == null || songData.Length == 0)
            {
                Debug.WriteLine("Invalid song data.");
                return false;
            }

            if (string.IsNullOrEmpty(originalFileName))
            {
                Debug.WriteLine("Invalid file name.");
                return false;
            }

            // 2. Extract Metadata (Optional but Recommended):
            //    - Use a library (like TagLib#) to read metadata (Title, Artist, Album, Duration, Genre, etc.) 
            //      directly from the songData byte array.
            //    - You'll need to add the TagLib# NuGet package: Install-Package TagLibSharp
            //    - Example using TagLib#:
            string title = string.Empty, artist = string.Empty, genre = string.Empty;
            TimeSpan duration = TimeSpan.Zero;
            try
            {
                using (var stream = new MemoryStream(songData))
                {
                    // Create a custom IFileAbstraction implementation for the MemoryStream
                    var fileAbstraction = new MemoryStreamFileAbstraction(originalFileName, stream);
                    using (var tagFile = TagLib.File.Create(fileAbstraction))
                    {
                        title = tagFile.Tag.Title;
                        artist = tagFile.Tag.FirstPerformer; // Or FirstArtist 
                        duration = tagFile.Properties.Duration;
                        genre = tagFile.Tag.Genres.First();
                    }
                }
            }
            catch (CorruptFileException ex)
            {
                Debug.WriteLine($"Error reading tags (corrupt file): {ex.Message}");
                // Handle case: maybe proceed without tags or reject the file
                return false;
            }
            catch (UnsupportedFormatException ex)
            {
                Debug.WriteLine($"Error reading tags (unsupported format): {ex.Message}");
                // Handle case: file format might be valid audio but not supported by TagLib#
                return false;
            }
            catch (Exception ex)
            {
                // Handle other potential errors during metadata extraction
                Debug.WriteLine($"Error extracting metadata: {ex.Message}");
                // Maybe set default values or return false 
            }


            // 3. Prepare Song Entity:
            //    - Create a new `Song` object.
            //    - Populate its properties using the extracted metadata (or defaults/placeholders if extraction failed).
            //    - Remember to link it to the correct User (e.g., using ActiveUser.Id from the MainApp form).
            //    - Set the `Data` property to the `songData` byte array (if storing in DB) or handle storage differently (see step 4).

            // 4. Data Storage:
            //    - Decide how to store the actual song file data (the byte array):
            //        a) Store directly in the database (e.g., VARBINARY(MAX) in SQL Server). Suitable for smaller files, simpler to manage. **Current Song model seems set up for this.**
            //        b) Store in the file system: Save the byte array as a file in a designated folder. Store only the *path* to the file in the database. Better for larger files, avoids bloating the database.
            //        c) Store in cloud storage (e.g., Azure Blob Storage, AWS S3): Upload the byte array to the cloud service. Store the URL/identifier in the database. Scalable and robust.
            //    - Implement the chosen storage mechanism.

            // 5. Save Song Metadata to Database:
            //    - Use the injected `_dbContext` to add the new `Song` entity.
            //    - Make sure all required fields are populated (Title, Duration, UserId, GenreId, Data/Path).
            //    - Example:
            var genreBusiness = new GenreBusiness(_dbContext);
            Genre? genreEntity = genreBusiness.FindByName(genre);
            if (genreEntity == null && !string.IsNullOrEmpty(genre))
            {
                genreEntity = new Genre { Name = genre };
                _dbContext.Genres.Add(genreEntity);
                genreEntity = genreBusiness.FindByName(genre);
            }
            if (string.IsNullOrEmpty(genre))
            {
                genreEntity = genreBusiness.FindByName("Unknown genre");
            }
            var newSong = new Song
            {
                Title = title ?? Path.GetFileNameWithoutExtension(originalFileName),
                Duration = (int)duration.TotalSeconds,
                UserId = user.Id,
                GenreId = genreEntity.Id,
                Data = songData,
                Artist = artist ?? "Unknown Artist"
            };
            _dbContext.Songs.Add(newSong);
            _dbContext.SaveChanges();

            // 6. Return Status:
            //    - Return true if the upload and saving process was successful.
            //    - Return false if any step failed (validation, metadata extraction, storage, database save).
            //    - Consider throwing specific exceptions for different failure types if more granular error handling is needed upstream.

            // Placeholder implementation - replace with your actual logic
            Debug.WriteLine($"Received song: {originalFileName}, Size: {songData.Length} bytes. Implement saving logic here.");
            // Simulate success for now
            return true; 

            // --- End of Business Logic --- 
        }
    }
}
