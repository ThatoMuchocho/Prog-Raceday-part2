using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.DTOs;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/results")]
public class ResultsController(ResultService results) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Capture(ResultRequest request)
    {
        var (status, body) = await results.CaptureAsync(request);
        return StatusCode(status, body);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Update(int id, ResultRequest request)
    {
        var (status, body) = await results.UpdateAsync(id, request);
        return body is null && status == 204 ? NoContent() : StatusCode(status, body);
    }

    [HttpGet("mine")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Mine()
    {
        var participantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await results.MineAsync(participantId));
    }

    [HttpGet("event/{eventId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> ForEvent(int eventId) => Ok(await results.ForEventAsync(eventId));
}
