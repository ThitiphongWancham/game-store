using GameStore.API.Data;
using GameStore.API.Features.Games.Constants;
using GameStore.API.Models;

namespace GameStore.API.Features.Games.GetGame;

public static class GetGameEndpoint
{
    public static void MapGetGame(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id}", (Guid id, GameStoreData data) =>
        {
            Game? game = data.GetGameById(id);
            return game is null
                ? Results.NotFound()
                : Results.Ok
                (
                    new GameDetailsDto
                    (
                        game.Id,
                        game.Name,
                        game.Genre.Id,
                        game.Price,
                        game.ReleaseDate,
                        game.Description
                    )
                );
        }).WithName(EndpointNames.GetGame);
    }
}
