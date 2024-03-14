namespace API_Aggregation.Dto.Weather;

public class RequestWeatherFromDbDto
{
    /// <summary>
    /// Latitude {-90, 90}
    /// </summary>
    public decimal Latitude { get; set; }
    /// <summary>
    /// Longitude {-180, 180}
    /// </summary>
    public decimal Longitude { get; set; }
    /// <summary>
    /// Valid Format: mm/dd/yyyy 
    /// </summary>
    public DateTime Date { get; set; }
}