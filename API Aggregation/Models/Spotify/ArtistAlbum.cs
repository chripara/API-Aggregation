namespace API_Aggregation.Models.Spotify;

public class ArtistAlbum
{
    public int AlbumId { get; set; }
    public Album Album { get; set; }
    public int ArtistId { get; set; }
    public Artist Artist { get; set; }
}