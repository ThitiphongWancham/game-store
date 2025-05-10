using GameStore.API.Data;
using GameStore.API.Features.Games;
using GameStore.API.Features.Genres;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("GameStore");

// Register the db-context as a scoped service
// builder.Services.AddDbContext<GameStoreContext>(options => options.UseSqlite(connString));
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

app.MapGet("/", () => "Hello World!, Welcome to GameStore!");

app.MapGames();
app.MapGenres();

await app.InitializeDb();

app.Run();
