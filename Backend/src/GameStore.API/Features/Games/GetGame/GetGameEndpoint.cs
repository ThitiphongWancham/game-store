using GameStore.API.Data;
using GameStore.API.Features.Games.Constants;
using GameStore.API.Models;

namespace GameStore.API.Features.Games.GetGame;

public static class GetGameEndpoint
{
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", (Guid id, GameStoreContext dbContext) =>
        {
            Game? game = dbContext.Games.Find(id);
            
            return game is null
                ? Results.NotFound()
                : Results.Ok
                (
                    new GameDetailsDto
                    (
                        game.Id,
                        game.Name,
                        game.GenreId,
                        game.Price,
                        game.ReleaseDate,
                        game.Description
                    )
                );
        }).WithName(EndpointNames.GetGame);
    }
}
