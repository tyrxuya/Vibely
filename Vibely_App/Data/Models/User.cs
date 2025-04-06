using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vibely_App.Data.Models
{
    [Table("users")]
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Required]
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [StringLength(32)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [Column("password")]
        public string Password { get; set; }

        [Required]
        [StringLength(32)]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        [Column("first_name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32)]
        [Column("last_name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(16)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column("is_premium")]
        public bool IsPremium { get; set; }

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly EndDate { get; set; }

        [Column("subscription_price")]
        public double SubscriptionPrice { get; set; }

        public ICollection<UserPlaylist> UserPlaylist { set { UserPlaylist = value; } }
        public ICollection<Song> Songs { set { Songs = value; } }
    }
}
