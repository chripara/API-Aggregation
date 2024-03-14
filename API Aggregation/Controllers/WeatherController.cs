using System.Net;
using API_Aggregation.Dto.Weather;
using API_Aggregation.Models.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API_Aggregation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly AppDbContext _context;
    private HttpClient client = new HttpClient();
    
    public WeatherController(
        AppDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    [Route("DownloadThisWeeksWeather")]
    public async Task<IActionResult> DownloadThisWeeksWeather([FromQuery] RequestDownloaddailyWeatherDto dto)
    {
        string responseBody="";
        
        using HttpResponseMessage response = await client.GetAsync(string.Format(
            @"https://api.openweathermap.org/data/3.0/onecall?lat={0}&lon={1}&exclude=currently,hourly,minutely&appid={2}",
            dto.Latitude, dto.Longitude, APIKeyConstants.WeatherAPIKey));
        response.EnsureSuccessStatusCode();
        
        responseBody = await response.Content.ReadAsStringAsync();
        
        ResponseWeatherDto responseWeatherDto = JsonConvert
            .DeserializeObject<ResponseWeatherDto>(responseBody
                .Replace("\r\n", String.Empty));

        foreach (var weatherDaily in responseWeatherDto.Daily)
        {
            if(!_context.Weathers.Any(a => a.Date == new Weather().UnixSecondsToDateTime(weatherDaily.Dt) 
               && a.Longitude == responseWeatherDto.Longitude 
               && a.Latitude == responseWeatherDto.Latitude))
                
            _context.Weathers.Add(new Weather
            {
                Date = new Weather().UnixSecondsToDateTime(weatherDaily.Dt),
                Longitude = responseWeatherDto.Longitude,
                Latitude = responseWeatherDto.Latitude,
                Humidity = weatherDaily.Humidity.ToString(),
                AtmosphericPressure = weatherDaily.Pressure.ToString(),
                PlaceName = responseWeatherDto.Timezone,
                WindDegree = weatherDaily.WindDeg.ToString(),
                DayInKelvin = weatherDaily.Temp.Day.ToString(),
                MorningInKelvin = weatherDaily.Temp.Morning.ToString(),
                NightInKelvin = weatherDaily.Temp.Night.ToString(),
                EveningTemperatureInKelvin = weatherDaily.Temp.Evening.ToString(),
                MaxTemperatureInKelvin = weatherDaily.Temp.Max.ToString(),
                MinTemperatureInKelvin = weatherDaily.Temp.Min.ToString(),
                FeelsLikeDayInKelvin = weatherDaily.FeelsLike.Day.ToString(),
                FeelsLikeEveningInKelvin = weatherDaily.FeelsLike.Evening.ToString(),
                FeelsLikeMorningInKelvin = weatherDaily.FeelsLike.Morning.ToString(),
                FeelsLikeNightInKelvin = weatherDaily.FeelsLike.Night.ToString(),
                WindInMeterPerSec = weatherDaily.WindSpeed.ToString()
            });
        }

        await _context.SaveChangesAsync();
        return Ok();
    }
    
    /// <summary>
    /// Get Weather details. Params: Latitude {-90, 90}, Longitude {-180, 180}, Exclude param seperated by comma (,): current, daily, minutely, hourly
    /// </summary>
    /// <param name="tickerSymbol"></param>
    /// <returns>A SharePriceResponse which contains the price of the share</returns>
    /// <response code="200">Returns 200 and the share price</response>
    /// <response code="400">Returns 400 if the query is invalid</response>
    [HttpGet]
    [Route("GetWeather")]
    public async Task<IActionResult> GetWeather([FromQuery]RequestWeatherDto dto)
    {
        string responseBody="";

        using HttpResponseMessage response = await client.GetAsync(string.Format(
            @"https://api.openweathermap.org/data/3.0/onecall?lat={0}&lon={1}&exclude={2}&appid={3}",
            dto.Latitude, dto.Longitude, dto.Exclude, APIKeyConstants.WeatherAPIKey));
    
        response.EnsureSuccessStatusCode();

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return Ok("Api openweathermap return BadRequest. Please review your fetching data and try again.");
    
        if (response.StatusCode == HttpStatusCode.Forbidden)
            return Ok("Api openweathermap return Forbidden. Something went wrong maybe try later.");
        
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Ok("Api openweathermap return Unauthorized. Something went wrong maybe try later.");

        if (!response.IsSuccessStatusCode)
            return Ok("Api openweathermap return an unknown error. Something went wrong maybe try later.");

        responseBody = await response.Content.ReadAsStringAsync();
        
        if (string.IsNullOrEmpty(responseBody))
            return Ok();
        
        return Ok(responseBody);
    }

    [HttpGet]
    [Route("GetWeatherFromDb")]
    public async Task<IActionResult> GetWeatherFromDb([FromQuery] RequestWeatherFromDbDto dto)
    {
        Weather weather = null;
        if(dto.Date != null)
            weather = await _context.Weathers.FirstOrDefaultAsync(p
                => p.Longitude == dto.Longitude
                   && p.Latitude == dto.Latitude
                   && p.Date.Date.Equals(dto.Date.Date));
        
        if(dto.Date == null)
            weather = await _context.Weathers.FirstOrDefaultAsync(p
                => p.Longitude == dto.Longitude
                   && p.Latitude == dto.Latitude);

        if (weather != null)
            return Ok(weather);

        return Ok("Didn't find any results. Please try again.");
    }
}