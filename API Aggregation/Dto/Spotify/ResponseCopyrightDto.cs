using Newtonsoft.Json;

namespace API_Aggregation.Dto.Spotify;

public class ResponseCopyrightDto
{
    [JsonProperty("text")]
    public string Text { get; set; }
    [JsonProperty("type")]
    public string Type { get; set; }
}