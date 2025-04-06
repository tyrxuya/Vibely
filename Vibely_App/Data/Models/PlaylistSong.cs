using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vibely_App.Data.Models
{
    [Table("playlists_songs")]
    public class PlaylistSong
    {
        [Required]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("playlist_id")]
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }

        [Required]
        [Column("song_id")]
        public int SongId { get; set; }
        public Song Song { get; set; }
    }
}
