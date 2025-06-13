using GameStore.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.API.Data;

public static class DataExtensions
{
    public static async Task InitializeDb(this WebApplication app)
    {
        await MigrateDbAsync(app);
        await SeedDbAsync(app);
        app.Logger.LogInformation("Initialized DB.");
    }

    private static async Task MigrateDbAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        await dbContext.Database.MigrateAsync();
    }

    private static async Task SeedDbAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

        if (!dbContext.Genres.Any())
        {
            await dbContext.Genres.AddRangeAsync(
                new Genre { Name = "Fighting" },
                new Genre { Name = "Kids and Family" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Roleplaying" },
                new Genre { Name = "Sports" }
            );

            await dbContext.SaveChangesAsync();
        }

        if (!dbContext.Games.Any())
        {
            const string defaultImageUri = "https://placehold.co/100";
            
            await dbContext.Games.AddRangeAsync(
                new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Street Fighter II",
                    GenreId = dbContext.Genres.First(g => g.Name == "Fighting").Id,
                    Price = 19.99m,
                    ReleaseDate = new DateOnly(1992, 7, 15),
                    Description = "Test description",
                    ImageUri = defaultImageUri,
                },
                new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "Final Fantasy XIV",
                    GenreId = dbContext.Genres.First(g => g.Name == "Roleplaying").Id,
                    Price = 59.99m,
                    ReleaseDate = new DateOnly(2010, 9, 30),
                    Description = "Test description",
                    ImageUri = defaultImageUri,
                },
                new Game
                {
                    Id = Guid.NewGuid(),
                    Name = "FIFA 23",
                    GenreId = dbContext.Genres.First(g => g.Name == "Sports").Id,
                    Price = 69.99m,
                    ReleaseDate = new DateOnly(2022, 9, 27),
                    Description = "Test description",
                    ImageUri = defaultImageUri,
                }
            );

            await dbContext.SaveChangesAsync();
        }
    }
}