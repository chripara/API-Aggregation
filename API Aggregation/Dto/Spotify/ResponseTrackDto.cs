using Newtonsoft.Json;

namespace API_Aggregation.Dto.Spotify;

public class ResponseTrackDto
{
    [JsonProperty("album")]
    public ResponseAlbumDto AlbumDto { get; set; }
    [JsonProperty("artists")]
    public ICollection<ResponseArtistDto> Artists { get; set; }
    [JsonProperty("available_markets")]
    public string[] AvailableMarkets { get; set; }
    [JsonProperty("disc_number")]
    public int DiscNumber { get; set; }
    [JsonProperty("duration_ms")]
    public int DurationMs { get; set; }
    [JsonProperty("explicit")]
    public bool Explicit { get; set; }
    [JsonProperty("external_ids")]
    public ResponseExternalIdDto ExternalId { get; set; }
    [JsonProperty("external_urls")]
    public ResponseExternalUrlDto ExternalUrl { get; set; }
    [JsonProperty("href")]
    public string Href { get; set; }
    [JsonProperty("id")]
    public string id { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("popularity")]
    public int Popularity { get; set; }
    [JsonProperty("preview_url")]
    public string PreviewUrl { get; set; }
    [JsonProperty("track_number")]
    public int TrackNumber { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("uri")]
    public string Uri { get; set; }
    [JsonProperty("is_local")]
    public bool IsLocal { get; set; }
}