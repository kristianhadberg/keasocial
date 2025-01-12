using System.Text;
using keasocial.Services;
using keasocial.Models;
using System.Text.Json;
using Moq;
using Moq.Protected;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace keasocial.Tests.UnitTests;

public class WeatherServiceTest
{
    [Fact]
    public async Task WeatherService_WeatherDto_ReturnsCorrectFormat()
    {
        // Arrange
        var mockConfigurationSection = new Mock<IConfigurationSection>();
        mockConfigurationSection.Setup(x => x.Value).Returns("mock_api_key");

        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.Setup(x => x.GetSection("ConnectionStrings:WeatherApiConnection")).Returns(mockConfigurationSection.Object);

        // mock api response
        var weatherApiResponse = new WeatherApi
        {
            weather = new List<Weather>
            {
                new Weather { description = "Clear sky" }
            },
            main = new MainWeather { temp = 20.5, humidity = 60 },
            wind = new Wind { speed = 5.2 }
        };

        var responseContent = JsonSerializer.Serialize(weatherApiResponse);

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(handlerMock.Object);

        var weatherService = new WeatherService(httpClient, mockConfiguration.Object);

        
        var result = await weatherService.GetWeatherAsync(); // Act

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Clear sky", result.description);
        Assert.Equal(20.5, result.temp);
        Assert.Equal(60, result.humidity);
        Assert.Equal(5.2, result.speed);
    }
}