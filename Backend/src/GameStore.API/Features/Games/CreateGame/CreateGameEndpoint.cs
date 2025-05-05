using GameStore.API.Data;
using GameStore.API.Features.Games.Constants;
using GameStore.API.Models;

namespace GameStore.API.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", (CreateGameDto game, GameStoreData data, GameDataLogger logger) =>
        {
            var genre = data.GetGenreById(game.GenreId);
            if (genre is null)
            {
                return Results.BadRequest("Invalid genre id");
            }

            var newGame = new Game
            {
                Genre = genre,
                GenreId = genre.Id,
                Name = game.Name,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Description = game.Description,
            };

            data.AddGame(newGame);
            logger.PrintGames();

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
