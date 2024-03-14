namespace API_Aggregation.Models.Spotify;

public class Artist
{
    public int Id { get; set; }
    public string SpotifyUrl { get; set; }
    public int TotalFollowers { get; set; }
    public ICollection<GenreArtist> Genres { get; set; }
    public string Href { get; set; }
    public string ArtistSpotifyId { get; set; }
    public ICollection<Image> Images { get; set; }
    public string Alias { get; set; }
    public int Popularity { get; set; }
    public string Type { get; set; }
    public ICollection<ArtistAlbum> Albums { get; set; }
}