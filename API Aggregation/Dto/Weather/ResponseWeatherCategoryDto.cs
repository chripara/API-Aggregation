using Newtonsoft.Json;

namespace API_Aggregation.Dto.Weather;

public class ResponseWeatherCategoryDto
{
    [JsonProperty("dt")]
    public int Dt { get; set; }
    [JsonProperty("sunrise")]
    public int Sunrise { get; set; }
    [JsonProperty("sunset")]
    public int Sunset { get; set; }
    [JsonProperty("moonrise")]
    public int Moonrise { get; set; }
    [JsonProperty("moonset")]
    public int Moonset { get; set; }
    [JsonProperty("temp")]
    public ResponseWeatherTempDto Temp { get; set; }
    [JsonProperty("feels_like")]
    public ResponseWeatherTempDto FeelsLike { get; set; }
    [JsonProperty("pressure")]
    public decimal Pressure  { get; set; }
    [JsonProperty("humidity")]
    public decimal Humidity  { get; set; }
    [JsonProperty("dew_point")]
    public decimal DewPoint  { get; set; }
    [JsonProperty("wind_speed")]
    public decimal WindSpeed  { get; set; }
    [JsonProperty("wind_deg")]
    public decimal WindDeg  { get; set; }
    [JsonProperty("wind_gust")]
    public decimal wind_gust  { get; set; }
    // [JsonProperty("weather")]
    // public decimal Weather  { get; set; }
}