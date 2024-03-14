using API_Aggregation.Models.Spotify;
using Newtonsoft.Json;

namespace API_Aggregation.Dto.Spotify;

public class ResponseArtistDto
{
    [JsonProperty("external_urls")]
    public ResponseExternalUrlDto ExternalUrls { get; set; }
    [JsonProperty("followers")]
    public ResponseFollowerDto Followers { get; set; }
    [JsonProperty("genres")]
    public string[] Genres { get; set; }
    [JsonProperty("href")]
    public string Href { get; set; }
    [JsonProperty("id")]
    public string Id { get; set; }
    [JsonProperty("images")]
    public ICollection<Image> Images { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("popularity")]
    public int Popularity { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
    [JsonProperty("uri")]
    public string Uri { get; set; }
}