namespace API_Aggregation.Dto.News;

public class RequestNewsFromDbDto
{
    /// <summary>
    /// Keywords or a phrase to search for.
    /// </summary>
    public string Q { get; set; } = "";
    /// <summary>
    /// Sort By PublishedAt
    /// </summary>
    public bool SortBy { get; set; } = false;
    /// <summary>
    /// Sort By PublishedAt Descending
    /// </summary>
    public bool SortByDesc { get; set; } = false;
    /// <summary>
    /// The number of results to return per page.
    /// Default: 100. Maximum: 100.
    /// </summary>
    public int? PageSize { get; set; } = null;
    /// <summary>
    /// Offset skip first:
    /// Default: 100. Maximum: 100.
    /// </summary>
    public int? Offset { get; set; } = null;
}