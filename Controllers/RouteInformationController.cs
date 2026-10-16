using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.DTOs;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/events/{eventId:int}/route")]
public class RouteInformationController(RouteService routes) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int eventId)
    {
        var (status, body) = await routes.GetAsync(eventId);
        return body is null ? NotFound() : StatusCode(status, body);
    }

    [HttpPut]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Upsert(int eventId, RouteRequest request)
    {
        var (status, body) = await routes.UpsertAsync(eventId, request);
        return StatusCode(status, body);
    }

    [HttpDelete]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Delete(int eventId)
    {
        var (status, body) = await routes.DeleteAsync(eventId);
        return body is null && status == 204 ? NoContent() : StatusCode(status, body);
    }
}
