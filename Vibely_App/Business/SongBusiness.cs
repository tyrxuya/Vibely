using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vibely_App.API;
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
    }
}
