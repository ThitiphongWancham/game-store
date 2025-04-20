using GameStore.API.Data;

namespace GameStore.API.Features.Games.DeleteGame;

public static class DeleteGameEndpoint
{
    public static void MapDeleteGame(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/{id}", (Guid id, GameStoreData data) =>
        {
            data.RemoveGame(id);
            return Results.NoContent();
        });
    }
}
