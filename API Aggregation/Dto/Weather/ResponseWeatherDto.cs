using API_Aggregation.Models.Weather;
using Newtonsoft.Json;

namespace API_Aggregation.Dto.Weather;

public class ResponseWeatherDto
{
    [JsonProperty("lat")]
    public decimal Latitude { get; set; }
    [JsonProperty("lon")]
    public decimal Longitude { get; set; }
    [JsonProperty("timezone")]
    public string Timezone { get; set; }
    [JsonProperty("daily")]
    public List<ResponseWeatherCategoryDto> Daily { get; set; }
}