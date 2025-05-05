using GameStore.API.Data;
using GameStore.API.Features.Games;
using GameStore.API.Features.Genres;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

// Registera dbcontext as a scoped service
// builder.Services.AddDbContext<GameStoreContext>(options => options.UseSqlite(connString));
builder.Services.AddSqlite<GameStoreContext>(connString);

builder.Services.AddSingleton<GameStoreData>();
builder.Services.AddSingleton<GameDataLogger>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!, Welcome to GameStore!");

app.MapGames();
app.MapGenres();

app.InitializeDb();

app.Run();
