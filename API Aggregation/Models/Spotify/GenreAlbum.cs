namespace API_Aggregation.Models.Spotify;

public class GenreAlbum
{
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    public int AlbumId { get; set; }
    public Album Album { get; set; }
}