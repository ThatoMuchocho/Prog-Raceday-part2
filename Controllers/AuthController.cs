using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.DTOs;
using RaceDay.Api.Services;

namespace RaceDay.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService auth, RaceDayDbContext db) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var (status, body) = await auth.RegisterAsync(request);
        return StatusCode(status, body);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var (status, body) = await auth.LoginAsync(request);
        return StatusCode(status, body);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var id = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await db.Users.AsNoTracking().Include(x => x.Role)
            .SingleOrDefaultAsync(x => x.UserID == id);

        return user is null
            ? NotFound()
            : Ok(new
            {
                user.UserID,
                user.FirstName,
                user.LastName,
                user.Email,
                Role = user.Role!.RoleName
            });
    }
}
