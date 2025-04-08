using System.ComponentModel.DataAnnotations;
using GameStore.API.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGameById";

List<Genre> genres =
[
    new Genre { Id = new Guid("7FF08C09-B1CF-472C-BEE5-376553D1B2D1"), Name = "Fighting" },
    new Genre { Id = new Guid("1666AAF5-F67F-4A64-9000-A41E84F9E39D"), Name = "Kids and Family" },
    new Genre { Id = new Guid("13BA7410-4051-43A2-B2A2-AB601EB19891"), Name = "Racing" },
    new Genre { Id = new Guid("362FF8CE-44D5-40DA-9D1E-A794474B1654"), Name = "Roleplaying" },
    new Genre { Id = new Guid("0EE921DE-FC06-4B9C-9E45-28E6905D4F77"), Name = "Sports" },
];

List<Game> games =
[
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Street Fighter II",
        Genre = genres[0],
        Price = 19.99m,
        ReleaseDate = new DateOnly(1992, 7, 15),
        Description = "Test description",
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Final Fantasy XIV",
        Genre = genres[3],
        Price = 59.99m,
        ReleaseDate = new DateOnly(2010, 9, 30),
        Description = "Test description",
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "FIFA 23",
        Genre = genres[4],
        Price = 69.99m,
        ReleaseDate = new DateOnly(2022, 9, 27),
        Description = "Test description",
    },
];

// ========================= GET =========================
app.MapGet("/", () => "Hello World!");

app.MapGet("/games", () => games.Select(game => new GameSummaryDto(
    game.Id,
    game.Name,
    game.Genre.Name,
    game.Price,
    game.ReleaseDate
)));

app.MapGet("/games/{id}", (Guid id) =>
{
    Game? game = games.Find(game => game.Id == id);
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
{
    return Results.Ok(genres.Select(genre => new GenreDto(genre.Id, genre.Name)));
});

// ========================= POST =========================
app.MapPost("/games", (CreateGameDto game) =>
{
    var genre = genres.Find(genre => genre.Id == game.GenreId);
    if (genre is null)
    {
        return Results.BadRequest("Invalid genre id");
    }

    var newGame = new Game
    {
        Id = Guid.NewGuid(),
        Genre = genre,
        Name = game.Name,
        Price = game.Price,
        ReleaseDate = game.ReleaseDate,
        Description = game.Description,
    };

    games.Add(newGame);

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
    var existingGame = games.Find(game => game.Id == id);

    if (existingGame is null)
    {
        return Results.NotFound();
    }

    var genre = genres.Find(genre => genre.Id == gameDto.GenreId);

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
    games.RemoveAll(game => game.Id == id);
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
