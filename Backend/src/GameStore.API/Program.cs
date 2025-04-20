using GameStore.API.Data;
using GameStore.API.Features.Games;
using GameStore.API.Features.Genres;

var builder = WebApplication.CreateBuilder(args);

// Register services here
builder.Services.AddSingleton<GameStoreData>();
builder.Services.AddSingleton<GameDataLogger>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!, Welcome to GameStore!");

app.MapGames();
app.MapGenres();

app.Run();
