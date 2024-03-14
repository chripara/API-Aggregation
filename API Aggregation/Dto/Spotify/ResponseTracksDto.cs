using Newtonsoft.Json;

namespace API_Aggregation.Dto.Spotify;

public class ResponseTracksDto
{
    [JsonProperty("href")]
    public string Href { get; set; }
    [JsonProperty("items")]
    public ICollection<ResponseTrackDto> TrackDtos { get; set; }
}