using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vibely_App.Data.Models
{
    [Table("users_playlists")]
    public class UserPlaylist
    {
        [Required]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("user_id")]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [Column("playlist_id")]
        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; }
    }
}
