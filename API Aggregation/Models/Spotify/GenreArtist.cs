namespace API_Aggregation.Models.Spotify;

public class GenreArtist
{
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    public int ArtistId { get; set; }
    public Artist Artist { get; set; }
}