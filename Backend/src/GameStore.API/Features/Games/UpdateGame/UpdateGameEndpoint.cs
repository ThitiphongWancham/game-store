using GameStore.API.Data;
using GameStore.API.Features.Games.Constants;
using GameStore.API.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Features.Games.UpdateGame;

public static class UpdateGameEndpoint
{
    public static void MapUpdateGame(this IEndpointRouteBuilder app)
    {
        app.MapPut("/{id:guid}",
            async (Guid id, [FromForm] UpdateGameDto game, GameStoreContext dbContext, ILogger<Program> logger,
                FileUploader fileUploader) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                if (game.ImageFile is not null)
                {
                    var fileUploadResult =
                        await fileUploader.UploadFileAsync(game.ImageFile, StorageNames.GameImagesFolder);

                    if (!fileUploadResult.IsSuccess)
                    {
                        return Results.BadRequest(new { message = fileUploadResult.ErrorMessage });
                    }

                    existingGame.ImageUri = fileUploadResult.FileUrl!;
                }

                existingGame.Name = game.Name;
                existingGame.GenreId = game.GenreId;
                existingGame.Price = game.Price;
                existingGame.ReleaseDate = game.ReleaseDate;
                existingGame.Description = game.Description;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            }).WithParameterValidation().DisableAntiforgery();
    }
}