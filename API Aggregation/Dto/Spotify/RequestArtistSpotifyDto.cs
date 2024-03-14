namespace API_Aggregation.Dto.Spotify;

public class RequestArtistSpotifyDto
{
    /// <summary>
    /// A comma-separated list of the Spotify IDs. For example: ids=4iV5W9uYEdYUVa79Axb7Rh,1301WleyT98MSxVHPZCA6M.
    /// Maximum: 100 IDs.
    /// </summary>
    public string ID { get; set; }
}