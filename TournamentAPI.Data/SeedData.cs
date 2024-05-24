using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;
using TournamentAPI.Data.Data;

public class SeedData
{
    public static async Task InitAsync(TournamentApiContext context)
    {
        if (await context.Tournaments.AnyAsync() || await context.Games.AnyAsync()) return;

        var tournaments = GenerateTournaments(2);
        await context.AddRangeAsync(tournaments);
        await context.SaveChangesAsync();

        var games = GenerateGames(5, tournaments);
        await context.AddRangeAsync(games);

        await context.SaveChangesAsync();
    }

    private static IEnumerable<Tournament> GenerateTournaments(int numberOfTournaments)
    {
        var tournaments = new List<Tournament>();

        for (int i = 0; i < numberOfTournaments; i++)
        {
            var tournament = new Tournament
            {
                Title = $"Tournament {i + 1}",
                StartDate = DateTime.Now.AddDays(i)
            };
            tournaments.Add(tournament);
        }
        return tournaments;
    }

    private static IEnumerable<Game> GenerateGames(int numberOfGames, IEnumerable<Tournament> tournaments)
    {
        var games = new List<Game>();

        foreach (var tournament in tournaments)
        {
            for (int i = 0; i < numberOfGames; i++)
            {
                var game = new Game
                {
                    Title = $"Game {i + 1} for {tournament.Title}",
                    Time = DateTime.Now.AddHours(i),
                    TournamentId = tournament.Id
                };
                games.Add(game);
            }
        }
        return games;
    }
}

