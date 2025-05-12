using GameStore.API.Data;
using GameStore.API.Features.Games.Constants;
using GameStore.API.Models;

namespace GameStore.API.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", async (CreateGameDto game, GameStoreContext dbContext, ILogger<Program> logger) =>
        {
            var validGenre = await dbContext.Genres.FindAsync(game.GenreId);

            if (validGenre == null)
                return Results.BadRequest("Genre not found.");

            var newGame = new Game
            {
                Name = game.Name,
                GenreId = game.GenreId,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Description = game.Description,
            };

            dbContext.Games.Add(newGame);

            await dbContext.SaveChangesAsync();

            logger.LogInformation(
                "Created game {NewGameName} with price ${NewGamePrice}", newGame.Name, newGame.Price);

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