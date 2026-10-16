using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.DTOs;

public record RegisterRequest(
    [property: Required, StringLength(50, MinimumLength = 2)] string FirstName,
    [property: Required, StringLength(50, MinimumLength = 2)] string LastName,
    [property: Required, EmailAddress, StringLength(150)] string Email,
    [property: Required, MinLength(8), StringLength(100)] string Password);

public record LoginRequest(
    [property: Required, EmailAddress] string Email,
    [property: Required] string Password);

public record EventRequest(
    [property: Required, StringLength(150)] string EventName,
    [property: StringLength(1000)] string? Description,
    DateOnly EventDate,
    [property: Required, StringLength(200)] string Location,
    [property: Range(typeof(decimal), "0.01", "100000")] decimal DistanceKm,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    [property: Required] string Status);

public record CategoryRequest(
    [property: Required, StringLength(100)] string CategoryName,
    [property: Required] string CategoryType,
    [property: StringLength(500)] string? Description,
    bool IsActive = true);

public record EventCategoryRequest(
    [property: Range(1, int.MaxValue)] int CategoryID,
    [property: Range(typeof(decimal), "0", "1000000")] decimal EntryFee,
    [property: Range(1, 1000000)] int MaxParticipants);

public record EnrolmentRequest([property: Range(1, int.MaxValue)] int EventCategoryID);

public record ResultRequest(
    [property: Range(1, int.MaxValue)] int EnrolmentID,
    [property: Range(1, int.MaxValue)] int? Position,
    TimeOnly? FinishTime,
    TimeOnly? ChipTime,
    [property: Range(typeof(decimal), "0.01", "1000")] decimal? Pace,
    bool IsOfficial = true);

public record RouteRequest(
    [property: Required, StringLength(150)] string RouteName,
    [property: StringLength(500)] string? RouteUrl,
    [property: StringLength(1000)] string? RouteDescription);

public record AuthResponse(int UserID, string FirstName, string LastName, string Email, string Role, string Token);

public record EventSummary(int EventID, string EventName, string? Description, DateOnly EventDate, string Location, decimal DistanceKm, string Status);

public record WeatherResponse(string Location, decimal Latitude, decimal Longitude, decimal? TemperatureC, decimal? WindSpeedKmh, string? Condition, string Source);
