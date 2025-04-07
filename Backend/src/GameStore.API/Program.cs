using GameStore.API.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<Game> games =
[
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = "Fighting",
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = "Roleplaying",
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = "Sports",
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27)
    },
];

app.MapGet("/", () => "Hello World!");
app.MapGet("/games", () => games);
app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = games.Find(game => game.Id == id);
    return game is null ? Results.NotFound() : Results.Ok(game);
}).WithName(GetGameEndpointName);

app.MapPost("/games", (Game game) =>
{
    game.Id = Guid.NewGuid();
    games.Add(game);

    return Results.CreatedAtRoute
    (
        GetGameEndpointName,
        new { id = game.Id },
        game
    );
}).WithParameterValidation();

app.MapPut("/games/{id}", (Guid id, Game updatedGame) =>
{
    var existingGame = games.Find(game => game.Id == id);
    if (existingGame is null)
    {
        return Results.NotFound();
    }
    existingGame.Name = updatedGame.Name;
    existingGame.Genre = updatedGame.Genre;
    existingGame.Price = updatedGame.Price;
    existingGame.ReleaseDate = updatedGame.ReleaseDate;

    return Results.NoContent();
}).WithParameterValidation();

app.MapDelete("games/{id}", (Guid id) =>
{
    games.RemoveAll(game => game.Id == id);
    return Results.NoContent();
});

app.Run();
