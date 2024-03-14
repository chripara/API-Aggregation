namespace API_Aggregation.Models.Weather;

public class Weather 
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Humidity { get; set; }
    public string DayInKelvin { get; set; }
    public string MinTemperatureInKelvin { get; set; }
    public string MaxTemperatureInKelvin { get; set; }
    public string EveningTemperatureInKelvin { get; set; }
    public string NightInKelvin { get; set; }
    public string MorningInKelvin { get; set; }
    public string FeelsLikeMorningInKelvin { get; set; }
    public string FeelsLikeEveningInKelvin { get; set; }
    public string FeelsLikeNightInKelvin { get; set; }
    public string FeelsLikeDayInKelvin { get; set; }
    //public string SeaLevel { get; set; }
    //public string GroundLevel { get; set; }
    public string WindInMeterPerSec { get; set; }
    public string WindDegree { get; set; }
    public string AtmosphericPressure { get; set; }
    public string PlaceName { get; set; }
    //public string WeatherMain { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    //public WeatherTypes WeatherType { get; set; }
    
    public DateTime UnixSecondsToDateTime(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
    }
}