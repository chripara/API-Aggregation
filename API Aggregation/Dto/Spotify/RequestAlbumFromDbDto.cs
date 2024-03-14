namespace API_Aggregation.Dto.Spotify;

public class RequestAlbumFromDbDto : PaginationSortingDto
{
    public string SpotifyAlbumId { get; set; } = "";
    public string Name { get; set; } = "";
    public DateTime MaxReleaseDate { get; set; } = DateTime.MaxValue;
    public DateTime MinReleaseDate { get; set; } = DateTime.MinValue;
    public bool OrderByName { get; set; }
    public bool OrderByTotalTracks { get; set; }
    public bool OrderByReleaseDate { get; set; }
    public bool IsASC { get; set; } = true;
}