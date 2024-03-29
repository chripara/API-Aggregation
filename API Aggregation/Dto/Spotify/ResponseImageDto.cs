﻿using Newtonsoft.Json;

namespace API_Aggregation.Dto.Spotify;

public class ResponseImageDto
{
    [JsonProperty("url")]
    public string Url { get; set; }
    [JsonProperty("height")]
    public int Height { get; set; }
    [JsonProperty("width")]
    public int Width { get; set; }
}