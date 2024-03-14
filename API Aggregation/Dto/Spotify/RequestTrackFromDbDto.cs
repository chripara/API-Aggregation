namespace API_Aggregation.Dto.Spotify;

public class RequestTrackFromDbDto : PaginationSortingDto
{
    public string SpotifyTrackId { get; set; } = "";
    public string Name { get; set; } = "";
    public bool OrderByName { get; set; }
    public bool IsASC { get; set; } = true;
}