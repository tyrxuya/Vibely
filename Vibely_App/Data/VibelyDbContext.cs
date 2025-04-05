using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Vibely_App.Data
{
    public class VibelyDbContext : DbContext
    {
        public VibelyDbContext()
        {
            Database.EnsureCreated();
        }

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
    }
}
