using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace _04._Weather_Dashboard;

class CurrentWeather
{
    [JsonPropertyName("time")]
    public DateTime Time { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
    
    [JsonPropertyName("windspeed")]
    public double WindSpeed { get; set; }
    
    [JsonPropertyName("weathercode")]
    public int WeatherCode { get; set; }
}

class Weather
{
    [JsonPropertyName("latitude")]
    public decimal Latitude { get; set; }
    
    [JsonPropertyName("longitude")]
    public decimal Longitude { get; set; }

    [JsonPropertyName("current_weather")]
    public CurrentWeather CurrentWeather { get; set; }
}

static class WeatherService
{
    static readonly HttpClient client = new HttpClient();
    
    public static async Task<Weather> GetWeatherAsync(string url)
    {
        try
        {
            Weather weather = await client.GetFromJsonAsync<Weather>(url);
            return weather;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new Weather();
        }
        
    }
}

static class WeatherInterpreter
{
    public static string DescribeWeather(Weather w)
    {
        CurrentWeather currentWeather = w.CurrentWeather;
        string weatherMapping = "";

        switch (currentWeather.WeatherCode)
        {
            case 0:
                weatherMapping = "Clear sky";
                break;
            case 1:
                weatherMapping = "Mainly clear";
                break;
            case 2:
                weatherMapping = "Partly cloudy";
                break;
            case 3:
                weatherMapping = "Overcast";
                break;
            case 45:
                weatherMapping = "Foggy";
                break;
            case 61:
                weatherMapping = "Light rain";
                break;
            case 63:
                weatherMapping = "Moderate rain";
                break;
            case 80:
                weatherMapping = "Rain showers";
                break;
            default:
                weatherMapping = "Unknown";
                break;
        }
        
        return $"{currentWeather.Temperature} C with wind at {currentWeather.WindSpeed} km/h ({weatherMapping})";
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        string url =
            @"https://api.open-meteo.com/v1/forecast?latitude=42.6977&longitude=23.3219&current_weather=true";
        Weather weather = await WeatherService.GetWeatherAsync(url);
        
        Console.WriteLine(weather.CurrentWeather.Time);
        
        string weatherInterpretation = WeatherInterpreter.DescribeWeather(weather);
        Console.WriteLine(weatherInterpretation);
    }
}