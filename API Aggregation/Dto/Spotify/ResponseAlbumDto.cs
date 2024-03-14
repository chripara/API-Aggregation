using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace API_Aggregation.Dto.Spotify;
//
// [DataContract]
public class ResponseAlbumDto
{
    [JsonProperty("album_type")]
    public string AlbumType { get; set; }
    [JsonProperty("artists")]
    public ICollection<ResponseArtistDto> ArtistDtos { get; set; }
    [JsonProperty("available_markets")]
    public string[] AvailableMarkets { get; set; }
    [JsonProperty("copyrights")]
    public ICollection<ResponseCopyrightDto> CopyrightDtos { get; set; }
    [JsonProperty("external_ids")]
    public ResponseExternalIdDto ExternalIds { get; set; }
    [JsonProperty("external_urls")]
    public ResponseExternalUrlDto ExternalUrls { get; set; }
    [JsonProperty("genres")]
    public string[] Genres { get; set; }
    [JsonProperty("href")]
    public string Href { get; set; }
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("Images")]
    public ICollection<ResponseImageDto> Images { get; set; }
    [JsonProperty("label")]
    public string Labels { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; } 
    [JsonProperty("popularity")]
    public string Popularity { get; set; }
    [JsonProperty("release_date")]
    public DateTime ReleaseDate { get; set; }
    [JsonProperty("release_date_precision")]
    public string ReleaseDatePrecision { get; set; }
    [JsonProperty("total_tracks")]
    public string TotalTracks { get; set; }
    [JsonProperty("tracks")]
    public ResponseTracksDto TrackDtos { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("uri")]
    public string Uri { get; set; }
}