namespace API_Aggregation.Models.Spotify;

public class Image
{
    public int Id { get; set; }
    public string Url { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public int? AlbumId { get; set; }
    public Album Album { get; set; }
    public int? TrackId { get; set; }
    public Track Track { get; set; }
    public int? ArtistId { get; set; }
    public Artist Artist { get; set; }
}