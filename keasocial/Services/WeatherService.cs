using keasocial.Models;
using keasocial.Services.Interfaces;
using System.Text.Json;
using keasocial.Dto;
using Microsoft.Extensions.Configuration;

namespace keasocial.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl;

    public WeatherService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        string apiKey = configuration.GetValue<string>("ConnectionStrings:WeatherApiConnection");
        Console.WriteLine(apiKey);
        _apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat=55.691827480278484&lon=12.554249909853317&units=metric&appid={apiKey}";
    }

    public async Task<GetWeatherDto> GetWeatherAsync()
    {
       
        var response = await _httpClient.GetAsync(_apiUrl);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<WeatherApi>(content);

        var weatherDto = new GetWeatherDto
        {
            description = result.weather[0].description,
            temp = (double)result.main.temp,
            humidity = result.main.humidity,
             speed = (double)result.wind.speed
        };

        return weatherDto;
    }
}