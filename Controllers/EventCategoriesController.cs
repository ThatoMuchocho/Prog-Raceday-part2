using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.DTOs;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/events/{eventId:int}/categories")]
public class EventCategoriesController(CategoryService categories) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int eventId) => Ok(await categories.ForEventAsync(eventId));

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Attach(int eventId, EventCategoryRequest request)
    {
        var (status, body) = await categories.AttachAsync(eventId, request);
        return StatusCode(status, body);
    }

    [HttpDelete("{eventCategoryId:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Detach(int eventId, int eventCategoryId)
    {
        var (status, body) = await categories.DetachAsync(eventId, eventCategoryId);
        return body is null && status == 204 ? NoContent() : StatusCode(status, body);
    }
}
