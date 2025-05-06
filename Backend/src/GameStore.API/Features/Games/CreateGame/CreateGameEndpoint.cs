using GameStore.API.Data;
using GameStore.API.Features.Games.Constants;
using GameStore.API.Models;

namespace GameStore.API.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", (CreateGameDto game, GameStoreContext dbContext) =>
        {
            var newGame = new Game
            {
                Name = game.Name,
                GenreId = game.GenreId,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Description = game.Description,
            };

            dbContext.Games.Add(newGame);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute
            (
                EndpointNames.GetGame,
                new { id = newGame.Id },
                new GameDetailsDto
                (
                    newGame.Id,
                    newGame.Name,
                    newGame.GenreId,
                    newGame.Price,
                    newGame.ReleaseDate,
                    newGame.Description
                )
            );
        }).WithParameterValidation();
    }
}
