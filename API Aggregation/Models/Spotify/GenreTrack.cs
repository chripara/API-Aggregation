namespace API_Aggregation.Models.Spotify;

public class GenreTrack
{
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    public int TrackId { get; set; }
    public Track Track { get; set; }
}