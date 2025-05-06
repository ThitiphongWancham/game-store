using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public static class DataExtensions
{
    public static void InitializeDb(this WebApplication app)
    {
        MigrateDb(app);
        SeedDb(app);
    }

    private static void MigrateDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        dbContext.Database.Migrate();
    }

    private static void SeedDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        if (!dbContext.Genres.Any())
        {
            dbContext.Genres.AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "Kids and Family" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Roleplaying" },
                new Genre { Name = "Sports" }
            );

            dbContext.SaveChanges();
        }

        if (!dbContext.Games.Any())
        {
            dbContext.Games.AddRange(
                new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Street Fighter II",
                    GenreId = dbContext.Genres.First(g => g.Name == "Fighting").Id,
                    Price = 19.99m,
                    ReleaseDate = new DateOnly(1992, 7, 15),
                    Description = "Test description",
                },
                new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Final Fantasy XIV",
                    GenreId = dbContext.Genres.First(g => g.Name == "Roleplaying").Id,
                    Price = 59.99m,
                    ReleaseDate = new DateOnly(2010, 9, 30),
                    Description = "Test description",
                },
                new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "FIFA 23",
                    GenreId = dbContext.Genres.First(g => g.Name == "Sports").Id,
                    Price = 69.99m,
                    ReleaseDate = new DateOnly(2022, 9, 27),
                    Description = "Test description",
                }
            );

            dbContext.SaveChanges();
        }
    }
}
