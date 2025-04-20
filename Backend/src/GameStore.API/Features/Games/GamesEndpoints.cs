using GameStore.API.Data;
using GameStore.API.Features.Games.CreateGame;
using GameStore.API.Features.Games.DeleteGame;
using GameStore.API.Features.Games.GetGame;
using GameStore.API.Features.Games.GetGames;
using GameStore.API.Features.Games.UpdateGame;

namespace GameStore.API.Features.Games;

public static class GamesEndpoints
{
    public static void MapGames(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/games");
        group.MapGetGames();
        group.MapGetGame();
        group.MapCreateGame();
        group.MapUpdateGame();
        group.MapDeleteGame();
    }
}
