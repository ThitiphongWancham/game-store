using GameStore.API.Data;
using GameStore.API.Features.Genres.GetGenres;

namespace GameStore.API.Features.Genres;

public static class GenresEndpoint
{
    public static void MapGenres(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/genres");

        group.MapGetGenres();
    }
}
