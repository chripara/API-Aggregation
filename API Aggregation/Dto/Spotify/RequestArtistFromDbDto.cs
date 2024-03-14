namespace API_Aggregation.Dto.Spotify;

public class RequestArtistFromDbDto : PaginationSortingDto
{
    public string SpotifyArtistId { get; set; } = "";
    public string Name { get; set; } = "";
    public DateTime MaxReleaseDate { get; set; } = DateTime.MaxValue;
    public DateTime MinReleaseDate { get; set; } = DateTime.MinValue;
    public bool OrderByAlias { get; set; }
    public bool OrderByTotalFollowers { get; set; }
    public bool OrderByPopularity { get; set; }
    public bool IsASC { get; set; } = true;
}