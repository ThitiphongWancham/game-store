using System.ComponentModel.DataAnnotations;
using GameStore.API.Data;
using GameStore.API.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGameById";

GameStoreData data = new();

// ========================= GET =========================
app.MapGet("/", () => "Hello World!");

app.MapGet("/games", () => data.GetGames().Select(game => new GameSummaryDto(
    game.Id,
    game.Name,
    game.Genre.Name,
    game.Price,
    game.ReleaseDate
)));

app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = data.GetGameById(id);
    return game is null
        ? Results.NotFound()
        : Results.Ok
        (
            new GameDetailsDto
            (
                game.Id,
                game.Name,
                game.Genre.Id,
                game.Price,
                game.ReleaseDate,
                game.Description
            )
        );
}).WithName(GetGameEndpointName);

app.MapGet("/genres", () =>
    data.GetGenres()
        .Select(genre => new GenreDto(genre.Id, genre.Name)));

// ========================= POST =========================
app.MapPost("/games", (CreateGameDto game) =>
{
    var genre = data.GetGenreById(game.GenreId);
    if (genre is null)
    {
        return Results.BadRequest("Invalid genre id");
    }

    var newGame = new Game
    {
        Genre = genre,
        Name = game.Name,
        Price = game.Price,
        ReleaseDate = game.ReleaseDate,
        Description = game.Description,
    };

    data.AddGame(newGame);

    return Results.CreatedAtRoute
    (
        GetGameEndpointName,
        new { id = newGame.Id },
        new GameDetailsDto
        (
            newGame.Id,
            newGame.Name,
            newGame.Genre.Id,
            newGame.Price,
            newGame.ReleaseDate,
            newGame.Description
        )
    );
}).WithParameterValidation();

// ========================= PUT =========================
app.MapPut("/games/{id}", (Guid id, UpdateGameDto gameDto) =>
{
    var existingGame = data.GetGameById(id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }

    var genre = data.GetGenreById(gameDto.GenreId);

    if (genre is null)
    {
        return Results.BadRequest("Invalid genre id");
    }

    existingGame.Name = gameDto.Name;
    existingGame.Genre = genre;
    existingGame.Price = gameDto.Price;
    existingGame.ReleaseDate = gameDto.ReleaseDate;
    existingGame.Description = gameDto.Description;

    return Results.NoContent();
}).WithParameterValidation();

// ========================= DELETE =========================
app.MapDelete("games/{id}", (Guid id) =>
{
    data.RemoveGame(id);
    return Results.NoContent();
});

app.Run();

public record GameDetailsDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description
);

public record GameSummaryDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);

public record CreateGameDto(
    [Required][StringLength(50)]
    string Name,

    Guid GenreId,

    [Range(1, 100)]
    decimal Price,

    DateOnly ReleaseDate,

    [Required][StringLength(500)]
    string Description
);

public record UpdateGameDto(
    [Required][StringLength(50)]
    string Name,

    Guid GenreId,

    [Range(1, 100)]
    decimal Price,

    DateOnly ReleaseDate,

    [Required][StringLength(500)]
    string Description
);

public record GenreDto(
    Guid Id,
    string Name
);
