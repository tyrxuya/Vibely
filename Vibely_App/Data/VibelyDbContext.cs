using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Vibely_App.Data.Models;

namespace Vibely_App.Data
{
    public class VibelyDbContext : DbContext
    {
        public VibelyDbContext()
        {
            Database.EnsureCreated();
            if (this.Genres.ToList().Count == 0)
            {
                this.Genres.Add(new Genre { Name = "Unknown genre" });
                this.SaveChanges();
            }
        }

        public VibelyDbContext(DbContextOptions options) : base(options) {}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets(typeof(Program).Assembly)
                .Build();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(config["ConnectionString"]);
            }
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistsSongs { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPlaylist> UsersPlaylists { get; set; }
    }
}
