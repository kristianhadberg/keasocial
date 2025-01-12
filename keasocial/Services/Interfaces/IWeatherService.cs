using keasocial.Dto;

namespace keasocial.Services.Interfaces;

public interface IWeatherService
{
    Task<GetWeatherDto> GetWeatherAsync();
}