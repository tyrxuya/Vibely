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
    public class PlaylistBusiness(VibelyDbContext dbContext) : IPlaylistBusiness
    {
        VibelyDbContext _dbContext = dbContext;

        public List<Playlist> GetAll()
        {
            return _dbContext.Playlists.ToList();
        }

        public void Add(Playlist playlist)
        {
            if (_dbContext.Playlists.Find(playlist.Id) != null)
            {
                return;
            }

            _dbContext.Playlists.Add(playlist);
            _dbContext.SaveChanges();
        }

        public Playlist? Find(int id)
        {
            return _dbContext.Playlists.Find(id);
        }

        //public Playlist? Find(Playlist playlist)
        //{
        //    return _dbContext.Playlists
        //        .FirstOrDefault(p => p.Id == playlist.Id && p.Title == playlist.Title);
        //}

        public void Remove(int id)
        {
            Playlist? playlist = _dbContext.Playlists.Find(id);

            if (playlist != null)
            {
                _dbContext.Playlists.Remove(playlist);
                _dbContext.SaveChanges();
            }
        }

        public void Update(int id, Playlist playlist)
        {
            Playlist? playlistToUpdate = _dbContext.Playlists.Find(id);

            if (playlistToUpdate != null)
            {
                _dbContext.Entry(playlistToUpdate).State = EntityState.Detached;

                playlistToUpdate.Title = playlist.Title;
                playlistToUpdate.Duration = playlist.Duration;

                _dbContext.Attach(playlistToUpdate);
                _dbContext.Entry(playlistToUpdate).State = EntityState.Modified;

                _dbContext.SaveChanges();
            }
        }
    }
}
