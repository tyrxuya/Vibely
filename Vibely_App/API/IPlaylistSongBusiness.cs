using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibely_App.Data.Models;

namespace Vibely_App.API
{
    public interface IPlaylistSongBusiness : IBusiness<PlaylistSong>
    {
        public List<Song> GetAllSongsInPlaylist(Playlist playlist);
        public PlaylistSong? FindByPlaylistAndSong(Playlist playlist, Song song);
    }
}
