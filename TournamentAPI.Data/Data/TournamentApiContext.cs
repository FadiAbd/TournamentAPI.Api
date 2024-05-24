using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data
{
    public class TournamentApiContext : DbContext
    {
        public TournamentApiContext(DbContextOptions<TournamentApiContext> options) : base(options)
        {

        }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tournament>()
        .HasMany(t => t.Games)
        .WithOne()
        .HasForeignKey(g => g.TournamentId)
        .OnDelete(DeleteBehavior.Cascade); // Cascading delete from Tournament to Games

            modelBuilder.Entity<Game>()
                .HasOne<Tournament>()
                .WithMany(t => t.Games)
                .HasForeignKey(g => g.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
