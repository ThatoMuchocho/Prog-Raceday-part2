# RaceDay – PROG6212 Portfolio of Evidence

Full-stack event management for South African road running, walking and cycling events.

**Part 2 (this submission):** ASP.NET Core 8 REST API, SQL Server, JWT role-based access, xUnit tests, GitHub Actions CI/CD.

## Demo accounts (after the SQL script + first API start in Development)

| Role | Email | Password |
| --- | --- | --- |
| Organiser | `jarred@raceday.co.za` | `Jarred@123!` |
| Participant | `thabo@raceday.co.za` | `Jarred@123!` |

Public `POST /api/auth/register` always creates a **Participant**. Do not expose a role picker.

## Setup

1. Install the .NET 8 SDK, Visual Studio 2022 (or VS Code) and SQL Server / LocalDB with SSMS.
2. In SSMS, run `docs/RaceDay_Database_script.sql` to create `RaceDayDB`.
3. Open `RaceDay.Api/appsettings.json` and confirm the connection string matches your instance (`(localdb)\\MSSQLLocalDB` is the default).
4. Restore, build and run:

```bash
dotnet restore RaceDay.sln
dotnet build RaceDay.sln
dotnet run --project RaceDay.Api
```

5. Open Swagger (`/swagger`). Log in, click **Authorize**, and paste `Bearer YOUR_TOKEN`.

## Tests

```bash
dotnet test RaceDay.sln
```

Tests use EF Core InMemory so CI does not need SQL Server. They cover Jarred the organiser, participant entries, results, validation and business rules.

## CI/CD

GitHub Actions workflow: `.github/workflows/ci.yml`  
The workflow restores, builds and runs the unit tests on every push to `main`.

**CI green-build screenshot:** add `docs/ci-green-build.png` after the first successful GitHub Actions run.

## Video presentation

Unlisted YouTube link (replace before ARC submission):

`https://youtu.be/YOUR_PART2_VIDEO`

The video must show the running API (Swagger), the code structure, and a live voiceover. AI-generated voices are not allowed.

## Repository layout

```
docs/                      Part 1 plan (ERD, API table, SQL script)
RaceDay.Api/               REST API
RaceDay.Tests/             xUnit tests (Jarred-focused scenarios)
.github/workflows/ci.yml   GitHub Actions
```

## Part 3 (next)

MVC client that consumes this API, Azure Blob Storage for event images, Docker containerisation.
