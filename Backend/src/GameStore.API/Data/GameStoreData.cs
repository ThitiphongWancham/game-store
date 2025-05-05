using GameStore.API.Models;

namespace GameStore.API.Data;

public class GameStoreData
{
    private readonly List<Genre> genres =
    [
        new Genre { Id = new Guid("7FF08C09-B1CF-472C-BEE5-376553D1B2D1"), Name = "Fighting" },
        new Genre { Id = new Guid("1666AAF5-F67F-4A64-9000-A41E84F9E39D"), Name = "Kids and Family" },
        new Genre { Id = new Guid("13BA7410-4051-43A2-B2A2-AB601EB19891"), Name = "Racing" },
        new Genre { Id = new Guid("362FF8CE-44D5-40DA-9D1E-A794474B1654"), Name = "Roleplaying" },
        new Genre { Id = new Guid("0EE921DE-FC06-4B9C-9E45-28E6905D4F77"), Name = "Sports" },
    ];

    private readonly List<Game> games;

    public GameStoreData()
    {
        games = [
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Street Fighter II",
                Genre = genres[0],
                GenreId = genres[0].Id,
                Price = 19.99m,
                ReleaseDate = new DateOnly(1992, 7, 15),
                Description = "Test description",
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "Final Fantasy XIV",
                Genre = genres[3],
                GenreId = genres[3].Id,
                Price = 59.99m,
                ReleaseDate = new DateOnly(2010, 9, 30),
                Description = "Test description",
            },
            new Game
            {
                Id = Guid.NewGuid(),
                Name = "FIFA 23",
                Genre = genres[4],
                GenreId = genres[4].Id,
                Price = 69.99m,
                ReleaseDate = new DateOnly(2022, 9, 27),
                Description = "Test description",
            },
        ];
    }

    public IEnumerable<Game> GetGames() => games;

    public Game? GetGameById(Guid id) => games.Find(game => game.Id == id);

    public void AddGame(Game newGame)
    {
        newGame.Id = Guid.NewGuid();
        games.Add(newGame);
    }

    public void RemoveGame(Guid id)
    {
        games.RemoveAll(game => game.Id == id);
    }

    public IEnumerable<Genre> GetGenres() => genres;

    public Genre? GetGenreById(Guid id) => genres.Find(genre => genre.Id == id);
}
