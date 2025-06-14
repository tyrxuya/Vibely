﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vibely_App.API;
using Vibely_App.Data;
using Vibely_App.Data.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Vibely_App.Business
{
    public class PlaylistSongBusiness(VibelyDbContext dbContext) : IPlaylistSongBusiness
    {
        VibelyDbContext _dbContext = dbContext;

        public List<PlaylistSong> GetAll()
        {
            return _dbContext.PlaylistsSongs
                .Include(ps => ps.Song)
                .Include(ps => ps.Playlist)
                .ToList();
        }

        public void Add(PlaylistSong playlistSong)
        {
            if (_dbContext.PlaylistsSongs.Find(playlistSong.Id) != null)
            {
                return;
            }

            _dbContext.PlaylistsSongs.Add(playlistSong);
            _dbContext.SaveChanges();

            var playlist = _dbContext.Playlists.Find(playlistSong.PlaylistId);
            var song = _dbContext.Songs.Find(playlistSong.SongId);
            _dbContext.Entry(playlist).State = EntityState.Detached;
            playlist.Duration += song.Duration;
            _dbContext.Attach(playlist);
            _dbContext.Entry(playlist).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public PlaylistSong? Find(int id)
        {
            return _dbContext.PlaylistsSongs.Find(id);
        }

        //public PlaylistSong? Find(PlaylistSong playlistSong)
        //{
        //    return Find(playlistSong.Id);
        //}

        public List<Song> GetAllSongsInPlaylist(Playlist playlist)
        {
            return _dbContext.PlaylistsSongs
                .Include(ps => ps.Song)
                .ThenInclude(s => s.Genre)
                .Include(ps => ps.Song)
                .ThenInclude(s => s.User)
                .Where(ps => ps.PlaylistId == playlist.Id)
                .Select(ps => ps.Song)
                .ToList();
        }

        public void Remove(int id)
        {
            PlaylistSong? playlistSong = _dbContext.PlaylistsSongs.Find(id);

            if (playlistSong != null)
            {
                _dbContext.PlaylistsSongs.Remove(playlistSong);
                _dbContext.SaveChanges();
            }
        }

        public void Update(int id, PlaylistSong playlistSong)
        {
            PlaylistSong? playlistSongToUpdate = _dbContext.PlaylistsSongs.Find(id);

            if (playlistSongToUpdate != null)
            {
                _dbContext.Entry(playlistSongToUpdate).State = EntityState.Detached;

                playlistSongToUpdate.PlaylistId = playlistSong.PlaylistId;
                playlistSongToUpdate.SongId = playlistSong.SongId;

                _dbContext.Attach(playlistSongToUpdate);
                _dbContext.Entry(playlistSongToUpdate).State = EntityState.Modified;

                _dbContext.SaveChanges();
            }
        }

        public PlaylistSong? FindByPlaylistAndSong(Playlist playlist, Song song)
        {
            return _dbContext.PlaylistsSongs
                .Include(ps => ps.Song)
                .Include(ps => ps.Playlist)
                .FirstOrDefault(ps => ps.PlaylistId == playlist.Id && ps.SongId == song.Id);
        }
    }
}
