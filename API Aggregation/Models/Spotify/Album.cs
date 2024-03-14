namespace API_Aggregation.Models.Spotify;

public class Album
{
    public int Id { get; set; }
    public string AlbumType { get; set; }
    public int TotalTracks { get; set; }
    public string externalUrls { get; set; }
    public string Href { get; set; }
    public ICollection<Image> Images { get; set; }
    public string Name { get; set; }
    public string SpotifyAlbumId { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Type { get; set; }
    public ICollection<ArtistAlbum> Artists { get; set; }
    public ICollection<Track> Tracks { get; set; }
}