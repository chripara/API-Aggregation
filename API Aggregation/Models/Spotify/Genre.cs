namespace API_Aggregation.Models.Spotify;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<GenreTrack> Tracks { get; set; }
    public ICollection<GenreAlbum> Albums { get; set; }
    public ICollection<GenreArtist> Artists { get; set; }
}