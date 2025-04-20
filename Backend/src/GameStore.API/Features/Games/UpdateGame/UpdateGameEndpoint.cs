using GameStore.API.Data;

namespace GameStore.API.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id}", (Guid id, UpdateGameDto gameDto, GameStoreData data) =>
        {
            var existingGame = data.GetGameById(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }

            var genre = data.GetGenreById(gameDto.GenreId);

            if (genre is null)
            {
                return Results.BadRequest("Invalid genre id");
            }

            existingGame.Name = gameDto.Name;
            existingGame.Genre = genre;
            existingGame.Price = gameDto.Price;
            existingGame.ReleaseDate = gameDto.ReleaseDate;
            existingGame.Description = gameDto.Description;

            return Results.NoContent();
        }).WithParameterValidation();
    }
}
