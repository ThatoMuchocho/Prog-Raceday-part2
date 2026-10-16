using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.DTOs;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(CategoryService categories) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get() => Ok(await categories.ListAsync());

    [HttpPost]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Create(CategoryRequest request)
    {
        var (status, body) = await categories.CreateAsync(request);
        return StatusCode(status, body);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Update(int id, CategoryRequest request)
    {
        var (status, body) = await categories.UpdateAsync(id, request);
        return body is null && status == 204 ? NoContent() : StatusCode(status, body);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Organiser")]
    public async Task<IActionResult> Delete(int id)
    {
        var (status, body) = await categories.DeleteAsync(id);
        return body is null && status == 204 ? NoContent() : StatusCode(status, body);
    }
}
