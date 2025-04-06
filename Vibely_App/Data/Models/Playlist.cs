using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vibely_App.Data.Models
{
    [Table("playlists")]
    public class Playlist
    {
        [Required]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        [Column("title")]
        public string Title { get; set; }

        [Column("duration")]
        public int Duration { get; set; }

        public ICollection<UserPlaylist> UserPlaylist { set { UserPlaylist = value; } }
        public ICollection<PlaylistSong> PlaylistSong { set { PlaylistSong = value; } }
    }
}
