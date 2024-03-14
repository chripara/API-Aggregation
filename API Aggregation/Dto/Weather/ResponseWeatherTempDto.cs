using Newtonsoft.Json;

namespace API_Aggregation.Dto.Weather;

public class ResponseWeatherTempDto
{
    [JsonProperty("day")]
    public decimal Day { get; set; }
    [JsonProperty("min")]
    public decimal Min { get; set; }
    [JsonProperty("max")]
    public decimal Max { get; set; }
    [JsonProperty("night")]
    public decimal Night { get; set; }
    [JsonProperty("eve")]
    public decimal Evening { get; set; }
    [JsonProperty("morn")]
    public decimal Morning { get; set; }
}