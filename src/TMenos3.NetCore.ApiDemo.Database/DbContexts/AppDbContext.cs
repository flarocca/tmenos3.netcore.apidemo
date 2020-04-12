using Microsoft.EntityFrameworkCore;
using TMenos3.NetCore.ApiDemo.Database.Entities;

namespace TMenos3.NetCore.ApiDemo.Database.DbContexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<LeagueTeam>()
                .HasKey(sc => new { sc.LeagueId, sc.TeamId});
        }

        public DbSet<League> Leagues { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Job> Jobs { get; set; }
    }
}
