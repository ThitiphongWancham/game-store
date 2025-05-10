using System;
using GameStore.API.Data;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Features.Genres.GetGenres;

public static class GetGenresEndpoint
{
    public static void MapGetGenres(this IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres.Select(genre => new GenreDto(genre.Id, genre.Name)).AsNoTracking().ToListAsync()
        );
    }
}