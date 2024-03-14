using API_Aggregation.Models.Weather;

namespace API_Aggregation.Test;

public class WeatherControllerTests
{
    [Fact]
    public void WeatherUnixSecondsToDateTime()
    {
        var weather = new Weather();
        
        // UnixSecondsToDateTime Calculate the Timestamp at UTC +0 and convert it at local
        // UTC +2 so 1710434174 UTC 3/14/2024, 6:36:14 PM  and convert it at 3/14/2024, 16:36:14
        Assert.Equal(weather.UnixSecondsToDateTime(1710434174), 
            new DateTime(2024, 3, 14, 16, 36, 14)); 
        
        Assert.NotEqual(weather.UnixSecondsToDateTime(1710434174), 
            new DateTime(2024, 3, 14, 18, 36, 14));
    }
}