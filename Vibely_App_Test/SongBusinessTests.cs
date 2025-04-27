using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Vibely_App.Data;
using Vibely_App.Business;
using Vibely_App.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Vibely_App_Test;

public class SongBusinessTests : DatabaseTestBase
{
    // Store IDs of prerequisite records created in setup
    private int _testUserId;
    private int _testGenreId;

    // Add prerequisite User and Genre records needed for Song tests
    [OneTimeSetUp]
    public async Task SetupPrerequisites()
    {
        await using var setupContext = new VibelyDbContext(CreateNewContextOptions());

        // Ensure a test User exists
        var testUser = await setupContext.Users.FirstOrDefaultAsync(u => u.Username == "_SongTestUser");
        if (testUser == null)
        {
            testUser = new User
            {
                Id = 99999, // High ID to avoid clashing with UserBusinessTests
                Username = "_SongTestUser",
                Password = "hashed_password",
                Email = "songtest@example.com",
                FirstName = "Song",
                LastName = "Tester",
                PhoneNumber = "0000000000",
                ProfilePicture = new byte[] { 0x01 },
                IsPremium = false,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)),
                SubscriptionPrice = 0
            };
            setupContext.Users.Add(testUser);
            await setupContext.SaveChangesAsync(); // SaveChanges to get the potentially DB-generated ID if not using explicit ID
        }
        _testUserId = testUser.Id;

        // Ensure a test Genre exists
        var testGenre = await setupContext.Genres.FirstOrDefaultAsync(g => g.Name == "_SongTestGenre");
        if (testGenre == null)
        {
            testGenre = new Genre { Id = 99999, Name = "_SongTestGenre" }; // High ID
            setupContext.Genres.Add(testGenre);
             await setupContext.SaveChangesAsync(); // SaveChanges to get the potentially DB-generated ID
        }
        _testGenreId = testGenre.Id;

        // Ensure "Unknown genre" exists (for UploadSong fallback)
        var unknownGenre = await setupContext.Genres.FirstOrDefaultAsync(g => g.Name == "Unknown genre");
        if (unknownGenre == null)
        {
            // Use a distinct ID or let the database generate one if Id isn't manually set
            unknownGenre = new Genre { Name = "Unknown genre" }; 
            setupContext.Genres.Add(unknownGenre);
            await setupContext.SaveChangesAsync();
             Console.WriteLine("[SongBusinessTests] Ensured 'Unknown genre' exists.");
        }

        Console.WriteLine($"[SongBusinessTests] Prerequisites ensured: UserId={_testUserId}, GenreId={_testGenreId}");
    }

    // Helper to create a valid Song associated with test User/Genre
    private Song CreateValidSong(int id, string titlePrefix = "TestSong")
    {
        return new Song
        {
            Id = id,
            Title = $"{titlePrefix}{id}",
            Artist = $"TestArtist{id}",
            Duration = 180 + id,
            GenreId = _testGenreId, // Use ID from setup
            UserId = _testUserId,   // Use ID from setup
            Data = new byte[] { (byte)id, 0x02, 0x03 } // Placeholder data
        };
    }

    // Helper to clean the Songs table
    private async Task CleanSongTable()
    {
        await using var cleanupContext = new VibelyDbContext(CreateNewContextOptions());
        // Need to be careful about FKs. Simple clear for now.
        var allSongs = await cleanupContext.Songs.ToListAsync();
        if (allSongs.Any())
        {
            cleanupContext.Songs.RemoveRange(allSongs);
            await cleanupContext.SaveChangesAsync();
        }
    }

    [Test]
    public async Task Add_ShouldPersistSong()
    {
        // Arrange
        await CleanSongTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);
        var newSong = CreateValidSong(1);

        // Act
        songBusiness.Add(newSong);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var addedSong = await assertContext.Songs.FindAsync(newSong.Id);
        addedSong.Should().NotBeNull();
        // Manual assertions (excluding navigation properties)
        addedSong.Id.Should().Be(newSong.Id);
        addedSong.Title.Should().Be(newSong.Title);
        addedSong.Artist.Should().Be(newSong.Artist);
        addedSong.Duration.Should().Be(newSong.Duration);
        addedSong.GenreId.Should().Be(newSong.GenreId);
        addedSong.UserId.Should().Be(newSong.UserId);
        addedSong.Data.Should().BeEquivalentTo(newSong.Data);
    }

    [Test]
    public async Task Add_WhenSongExists_ShouldNotAddDuplicate()
    {
        // Arrange
        await CleanSongTable();
        var existingSong = CreateValidSong(2);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Songs.Add(existingSong);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);
        var duplicateSong = CreateValidSong(2); // Same ID

        // Act
        songBusiness.Add(duplicateSong);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var songs = await assertContext.Songs.Where(s => s.Id == existingSong.Id).ToListAsync();
        songs.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAll_ShouldReturnAllAddedSongsWithIncludes()
    {
        // Arrange
        await CleanSongTable();
        var song1 = CreateValidSong(3);
        var song2 = CreateValidSong(4);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Songs.AddRange(song1, song2);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);

        // Act
        var songs = songBusiness.GetAll();

        // Assert
        songs.Should().NotBeNull();
        songs.Should().HaveCount(2);
        // Check one song in detail, including navigation properties
        var retrievedSong1 = songs.FirstOrDefault(s => s.Id == song1.Id);
        retrievedSong1.Should().NotBeNull();
        retrievedSong1.Title.Should().Be(song1.Title);
        retrievedSong1.Genre.Should().NotBeNull(); // Check include worked
        retrievedSong1.Genre.Id.Should().Be(_testGenreId);
        retrievedSong1.User.Should().NotBeNull(); // Check include worked
        retrievedSong1.User.Id.Should().Be(_testUserId);
        // Check presence of the other song ID
        songs.Should().ContainSingle(s => s.Id == song2.Id);
    }

    [Test]
    public async Task Find_WithExistingId_ShouldReturnSong()
    {
        // Arrange
        await CleanSongTable();
        var song = CreateValidSong(5);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Songs.Add(song);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);

        // Act
        var foundSong = songBusiness.Find(song.Id);

        // Assert
        foundSong.Should().NotBeNull();
        foundSong.Id.Should().Be(song.Id);
        foundSong.Title.Should().Be(song.Title); // Add more property checks if needed
    }

    [Test]
    public async Task Find_WithNonExistingId_ShouldReturnNull()
    {
        // Arrange
        await CleanSongTable();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);

        // Act
        var foundSong = songBusiness.Find(999);

        // Assert
        foundSong.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithExistingId_ShouldDeleteSong()
    {
        // Arrange
        await CleanSongTable();
        var song = CreateValidSong(6);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Songs.Add(song);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);

        // Act
        songBusiness.Remove(song.Id);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var deletedSong = await assertContext.Songs.FindAsync(song.Id);
        deletedSong.Should().BeNull();
    }

    [Test]
    public async Task Remove_WithNonExistingId_ShouldDoNothing()
    {
        // Arrange
        await CleanSongTable();
        var song = CreateValidSong(7);
        int initialCount = 0;
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Songs.Add(song);
            await setupContext.SaveChangesAsync();
            initialCount = await setupContext.Songs.CountAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);

        // Act
        songBusiness.Remove(998);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.Songs.Count().Should().Be(initialCount);
        (await assertContext.Songs.AnyAsync(s => s.Id == song.Id)).Should().BeTrue();
    }

    [Test]
    public async Task Update_ShouldModifySong()
    {
        // Arrange
        await CleanSongTable();
        var originalSong = CreateValidSong(8);
        await using (var setupContext = new VibelyDbContext(CreateNewContextOptions()))
        {
            setupContext.Songs.Add(originalSong);
            await setupContext.SaveChangesAsync();
        }

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);

        var updatedSongData = CreateValidSong(8); // Same ID
        updatedSongData.Title = "UpdatedSongTitle";
        updatedSongData.Duration = 555;
        updatedSongData.Data = new byte[] { 0xAA, 0xBB };

        // Act
        songBusiness.Update(originalSong.Id, updatedSongData);

        // Assert
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var updatedSong = await assertContext.Songs.FindAsync(originalSong.Id);
        updatedSong.Should().NotBeNull();
        updatedSong.Title.Should().Be("UpdatedSongTitle");
        updatedSong.Duration.Should().Be(555);
        updatedSong.Data.Should().BeEquivalentTo(new byte[] { 0xAA, 0xBB });
        // Check unchanged FKs
        updatedSong.GenreId.Should().Be(originalSong.GenreId);
        updatedSong.UserId.Should().Be(originalSong.UserId);
    }
    
    // Note: Testing UploadSong is more complex due to TagLib# dependency
    // and potential file interactions. It might require mocking or a different setup.

    // --- UploadSong Tests ---

    private async Task<User> GetTestUserFromDb()
    {
        // Helper to get the actual User entity for passing to UploadSong
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        return await context.Users.FindAsync(_testUserId);
    }

    [Test]
    public async Task UploadSong_WithValidMp3_ShouldAddSongAndReturnTrue()
    {
        // Arrange
        await CleanSongTable(); // Start clean
        var user = await GetTestUserFromDb();
        user.Should().NotBeNull("Test user must exist for upload test");

        var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "sample.mp3");
        File.Exists(filePath).Should().BeTrue("sample.mp3 must exist in Resources and be copied to output");
        var songData = await File.ReadAllBytesAsync(filePath);

        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);

        // Act
        var result = songBusiness.UploadSong(user, songData, "sample.mp3");

        // Assert
        result.Should().BeTrue();

        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        var addedSongs = await assertContext.Songs.Where(s => s.UserId == user.Id).ToListAsync();
        addedSongs.Should().HaveCount(1);
        var addedSong = addedSongs.First();
        
        // Assert metadata extracted by TagLib# (adjusted for specific expected tags)
        addedSong.Title.Should().Be("Test MP3 Title"); // Should get title from tags
        addedSong.Artist.Should().Be("Test MP3 Artist"); // Should get artist from tags
        addedSong.Duration.Should().BeGreaterThan(0); // Should get duration from tags
        // Check if the specific genre was found or created
        addedSong.GenreId.Should().BeGreaterThan(0); 
        var genre = await assertContext.Genres.FindAsync(addedSong.GenreId);
        genre.Should().NotBeNull();
        genre.Name.Should().Be("Test MP3 Genre"); // Expect the genre from the tag
        
        addedSong.Data.Should().BeEquivalentTo(songData);
    }
    
    [Test]
    public async Task UploadSong_WithNonAudioFile_ShouldReturnFalseAndNotAddSong()
    {
        // Arrange
        await CleanSongTable(); 
        var user = await GetTestUserFromDb();
        user.Should().NotBeNull();
        
        var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "not_audio.txt");
        File.Exists(filePath).Should().BeTrue("not_audio.txt must exist in Resources and be copied to output");
        var fileData = await File.ReadAllBytesAsync(filePath);
        
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);
        var initialSongCount = await context.Songs.CountAsync();
        
        // Act
        var result = songBusiness.UploadSong(user, fileData, "not_audio.txt");
        
        // Assert
        result.Should().BeFalse();
        
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.Songs.Count().Should().Be(initialSongCount);
    }
    
    [Test]
    public async Task UploadSong_WithNullData_ShouldReturnFalse()
    {
        // Arrange
        var user = await GetTestUserFromDb();
        user.Should().NotBeNull();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);
        var initialSongCount = await context.Songs.CountAsync();
        
        // Act
        var result = songBusiness.UploadSong(user, null, "null_data.mp3");
        
        // Assert
        result.Should().BeFalse();
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.Songs.Count().Should().Be(initialSongCount);
    }
    
    [Test]
    public async Task UploadSong_WithEmptyData_ShouldReturnFalse()
    {
        // Arrange
        var user = await GetTestUserFromDb();
        user.Should().NotBeNull();
        await using var context = new VibelyDbContext(CreateNewContextOptions());
        var songBusiness = new SongBusiness(context);
        var initialSongCount = await context.Songs.CountAsync();
        
        // Act
        var result = songBusiness.UploadSong(user, Array.Empty<byte>(), "empty_data.mp3");
        
        // Assert
        result.Should().BeFalse();
        await using var assertContext = new VibelyDbContext(CreateNewContextOptions());
        assertContext.Songs.Count().Should().Be(initialSongCount);
    }
    
    // TODO: Add test for MP3 missing tags to check default handling
    // TODO: Add test for case where genre needs to be created by UploadSong
} 