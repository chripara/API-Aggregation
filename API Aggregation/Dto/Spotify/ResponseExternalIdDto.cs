using Newtonsoft.Json;

namespace API_Aggregation.Dto.Spotify;

public class ResponseExternalIdDto
{
    [JsonProperty("ups")]
    public string Ups { get; set; }
}