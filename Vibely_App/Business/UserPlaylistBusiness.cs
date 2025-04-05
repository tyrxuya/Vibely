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
    public class UserPlaylistBusiness(VibelyDbContext dbContext) : IUserPlaylistBusiness
    {
        VibelyDbContext _dbContext = dbContext;

        public List<UserPlaylist> GetAll()
        {
            return _dbContext.UsersPlaylists
                .Include(up => up.User)
                .Include(up => up.Playlist)
                .ToList();
        }

        public void Add(UserPlaylist userPlaylist)
        {
            if (_dbContext.UsersPlaylists.Find(userPlaylist.Id) != null)
            {
                return;
            }

            _dbContext.UsersPlaylists.Add(userPlaylist);
            _dbContext.SaveChanges();
        }

        public UserPlaylist? Find(int id)
        {
            return _dbContext.UsersPlaylists.Find(id);
        }

        public void Remove(int id)
        {
            UserPlaylist? userPlaylist = _dbContext.UsersPlaylists.Find(id);

            if (userPlaylist != null)
            {
                _dbContext.UsersPlaylists.Remove(userPlaylist);
                _dbContext.SaveChanges();
            }
        }

        public void Update(int id, UserPlaylist userPlaylist)
        {
            UserPlaylist? userPlaylistToUpdate = _dbContext.UsersPlaylists.Find(id);

            if (userPlaylistToUpdate != null)
            {
                _dbContext.Entry(userPlaylistToUpdate).State = EntityState.Detached;

                userPlaylistToUpdate.PlaylistId = userPlaylist.PlaylistId;
                userPlaylistToUpdate.UserId = userPlaylist.UserId;

                _dbContext.Attach(userPlaylistToUpdate);
                _dbContext.Entry(userPlaylistToUpdate).State = EntityState.Modified;

                _dbContext.SaveChanges();
            }
        }
    }
}
