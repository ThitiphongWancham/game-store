using GameStore.API.Data;
using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Features.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", (Guid id, GameStoreContext dbContext) =>
        {
            // This way is inefficient because you have to query data and then delete it.
            // Game? game = dbContext.Games.Find(id);
            // if (game == null)
            // {
            //     return Results.NotFound();
            // }
            // dbContext.Games.Remove(game);
            // dbContext.SaveChanges();

            // Use batch-delete instead. It has no changes tracking involve. 
            // So, it automatically sends the delete request to the db. That's why you don't have to call SaveChanges()
            dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDelete();

            return Results.NoContent();
        });
    }
}
