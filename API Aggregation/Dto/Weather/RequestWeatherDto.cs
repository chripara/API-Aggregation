using System.ComponentModel.DataAnnotations;

namespace API_Aggregation.Dto.Weather;

public class RequestWeatherDto
{
    /// <summary>
    /// Keywords or a phrase to search for.
    /// </summary>
    public string Latitude { get; set; }
    
    /// <summary>
    /// Keywords or a phrase to search for.
    /// </summary>
    public string Longitude { get; set; }
    
    /// <summary>
    /// Keywords or a phrase to search for.
    /// </summary>
    public string? Exclude { get; set; }
}