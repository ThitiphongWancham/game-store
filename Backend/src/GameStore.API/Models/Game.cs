using System.ComponentModel.DataAnnotations;

namespace GameStore.API.Models;

public class Game
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public Genre? Genre { get; set; }

    public Guid GenreId {get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

    [MaxLength(500)]
    public required string Description { get; set; }
    
    [MaxLength(100)]
    public required string ImageUri { get; set; }
}
