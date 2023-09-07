using eBasketballScrapper.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace eBasketballScrapper.Infrastructure.Data
{
    public class eBasketballDbContext : DbContext
    {
        public DbSet<Game> Matches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost; Port=3306; initial catalog =eBasketballAI; uid=root; pwd=bluegreendark2!", new MySqlServerVersion(new Version(8, 0, 34)));
        }

    }
}