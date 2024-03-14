namespace API_Aggregation.Models.Spotify;

public class Track
{
    public int Id { get; set; }
    public string ExternalUrl { get; set; }
    public string Name { get; set; }
    public int? Popularity { get; set; }
    public string Type { get; set; }
    public string SpotifyTrackID { get; set; }
    public string Href { get; set; }
    public int AlbumId { get; set; }
    public Album Album { get; set; }
    public ICollection<GenreTrack> Genres { get; set; }
    public string AvailableMarkets { get; set; }
    public ICollection<Image> Images { get; set; }
}