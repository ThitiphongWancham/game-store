using GameStore.API.Data;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Features.Games.GetGames;

public static class GetGamesEndpoint
{
    public static void MapGetGames(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (GameStoreContext dbContext, [AsParameters] GetGamesDto request) =>
            {
                var skipCount = (request.PageNumber - 1) * request.PageSize;

                var filteredGames = dbContext.Games.Where(game =>
                    string.IsNullOrWhiteSpace(request.Name) || EF.Functions.Like(game.Name, $"%{request.Name}%"));

                var gamesOnPage = await filteredGames
                    .OrderBy(game => game.Name)
                    .Skip(skipCount)
                    .Take(request.PageSize)
                    .Include(game => game.Genre)
                    .Select(game => new GameSummaryDto(
                        game.Id,
                        game.Name,
                        game.Genre!.Name,
                        game.Price,
                        game.ReleaseDate,
                        game.ImageUri
                    ))
                    .AsNoTracking()
                    .ToListAsync();

                var totalGames = await filteredGames.CountAsync();
                var totalPages = (int)Math.Ceiling((double)totalGames / request.PageSize);

                return new GamesPageDto(totalPages, gamesOnPage);
            }
        );
    }
}