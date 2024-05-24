using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace TournamentAPI.Data.Data
{
    public class TournamentApiContextFactory : IDesignTimeDbContextFactory<TournamentApiContext>
    {
        public TournamentApiContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<TournamentApiContext>();
            options.UseSqlServer("Not required for scaffolding");
            return new TournamentApiContext(options.Options);
        }
    }
}
