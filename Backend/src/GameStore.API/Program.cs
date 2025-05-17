using GameStore.API.Data;
using GameStore.API.Features.Games;
using GameStore.API.Features.Genres;
using GameStore.API.Shared.Timing;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

// Register the db-context as a scoped service
// builder.Services.AddDbContext<GameStoreContext>(options => options.UseSqlite(connString));
builder.Services.AddSqlite<GameStoreContext>(connString);

// In order to use HttpLogging middleware, we need to inject a service and enable it in appsettings.json
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode | HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!, Welcome to GameStore!");

app.MapGames();
app.MapGenres();

// Built-in middleware
app.UseHttpLogging();
// Custom middleware
app.UseMiddleware<RequestTimingMiddleware>();

await app.InitializeDb();

app.Run();