using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Models;

namespace RaceDay.Api.Data;

public static class DevelopmentSeeder
{
    public const string DemoPassword = "Jarred@123!";

    public static async Task ReplaceDemoPasswordHashesAsync(RaceDayDbContext db)
    {
        var demoUsers = await db.Users
            .Where(x => x.PasswordHash.StartsWith("DEMO_HASH"))
            .ToListAsync();

        if (demoUsers.Count == 0)
            return;

        var hash = BCrypt.Net.BCrypt.HashPassword(DemoPassword);
        foreach (var user in demoUsers)
            user.PasswordHash = hash;

        await db.SaveChangesAsync();
    }

    public static async Task EnsureInMemorySampleAsync(RaceDayDbContext db)
    {
        if (await db.Users.AnyAsync())
            return;

        db.Roles.AddRange(
            new Role { RoleID = 1, RoleName = "Organiser", Description = "Manages events." },
            new Role { RoleID = 2, RoleName = "Participant", Description = "Enters events." });
        await db.SaveChangesAsync();
    }
}
