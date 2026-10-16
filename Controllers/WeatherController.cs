using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController(IWeatherService weather) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get([FromQuery] string location, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(location))
            return BadRequest(new { message = "Provide a South African city such as Cape Town, Soweto or Durban." });

        try
        {
            var result = await weather.GetAsync(location, cancellationToken);
            return result is null
                ? NotFound(new { message = "Location is not in the RaceDay city list." })
                : Ok(result);
        }
        catch (HttpRequestException)
        {
            return StatusCode(503, new { message = "Live weather is temporarily unavailable." });
        }
    }
}
