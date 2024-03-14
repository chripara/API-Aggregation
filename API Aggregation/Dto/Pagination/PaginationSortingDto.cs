namespace API_Aggregation.Dto.Spotify;

public class PaginationSortingDto
{
    public int MaxPerPage { get; set; } = 100;
    public int Offset { get; set; } = 0;
}