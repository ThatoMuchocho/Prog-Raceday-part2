using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.DTOs;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController(EventService events) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get() => Ok(await events.ListAsync());

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOne(int id)
    {
        var entity = await events.GetAsync(id);
        return entity is null ? NotFound() : Ok(entity);
    }

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Create(EventRequest request)
    {
        var organiserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (status, body) = await events.CreateAsync(request, organiserId);
        return StatusCode(status, body);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Update(int id, EventRequest request)
    {
        var (status, body) = await events.UpdateAsync(id, request);
        return body is null && status == 204 ? NoContent() : StatusCode(status, body);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Delete(int id)
    {
        var (status, body) = await events.DeleteAsync(id);
        return body is null && status == 204 ? NoContent() : StatusCode(status, body);
    }
}
