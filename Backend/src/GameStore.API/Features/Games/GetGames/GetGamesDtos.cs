namespace GameStore.API.Features.Games.GetGames;

public record GameSummaryDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate,
    string ImageUri
);

// Input DTO (parameter)
public record GetGamesDto(int PageNumber = 1, int PageSize = 5, string? Name = null);

// Response DTO
public record GamesPageDto(int TotalPages, IEnumerable<GameSummaryDto> Data);