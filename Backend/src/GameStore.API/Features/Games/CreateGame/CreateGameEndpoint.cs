using GameStore.API.Data;
using GameStore.API.Features.Games.Constants;
using GameStore.API.Models;
using GameStore.API.Shared.FileUpload;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Features.Games.CreateGame;

public static class CreateGameEndpoint
{
    private const string DefaultImageUri = "https://placehold.co/100";

    public static void MapCreateGame(this IEndpointRouteBuilder app)
    {
        app.MapPost("/",
            async ([FromForm] CreateGameDto game, GameStoreContext dbContext, ILogger<Program> logger,
                FileUploader fileUploader) =>
            {
                var validGenre = await dbContext.Genres.FindAsync(game.GenreId);

                if (validGenre == null)
                    return Results.BadRequest("Genre not found.");

                string? imageUri = null;

                if (game.ImageFile is not null)
                {
                    var fileUploadResult =
                        await fileUploader.UploadFileAsync(game.ImageFile, StorageNames.GameImagesFolder);

                    if (!fileUploadResult.IsSuccess)
                    {
                        return Results.BadRequest(new { message = fileUploadResult.ErrorMessage });
                    }

                    imageUri = fileUploadResult.FileUrl;
                }

                var newGame = new Game
                {
                    Name = game.Name,
                    GenreId = game.GenreId,
                    Price = game.Price,
                    ReleaseDate = game.ReleaseDate,
                    Description = game.Description,
                    ImageUri = imageUri ?? DefaultImageUri,
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
                        newGame.Description,
                        newGame.ImageUri
                    )
                );
            }).WithParameterValidation().DisableAntiforgery();

        /*  
            Antiforgery tries to prevent CSRF attacks. CSRF = Cross Site Request Forgery
            - It is a type of attack where a malicious website tricks a user's browser to performing actions on 
            another website without their consent.
            - To achive this, a malicious website will try to use the user's authentication cookie, which browsers
            will send automatically, to try to make unauthorized requests to your website.
            - Your API would processes the request because it appears to be valid.
            - Fortunately, our gamestore app does not accept cookies, so we don't need anti-forgery.
        */
    }
}