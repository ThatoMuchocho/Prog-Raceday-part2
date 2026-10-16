using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.DTOs;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/enrolments")]
public class EnrolmentsController(EnrolmentService enrolments) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Enter(EnrolmentRequest request)
    {
        var participantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (status, body) = await enrolments.EnterAsync(participantId, request.EventCategoryID);
        return StatusCode(status, body);
    }

    [HttpGet("mine")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Mine()
    {
        var participantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await enrolments.MineAsync(participantId));
    }

    [HttpGet]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> All() => Ok(await enrolments.AllAsync());

    [HttpGet("event/{eventId:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> ForEvent(int eventId) => Ok(await enrolments.ForEventAsync(eventId));

    [HttpPut("{id:int}/cancel")]
    [Authorize(Roles = "Participant")]
    public async Task<IActionResult> Cancel(int id)
    {
        var participantId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var (status, body) = await enrolments.CancelOwnAsync(id, participantId);
        return body is null && status == 204 ? NoContent() : StatusCode(status, body);
    }
}
