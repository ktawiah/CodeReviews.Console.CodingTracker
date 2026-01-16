# Coding Tracker (Dapper + Spectre.Console)

A console app to log coding sessions using Dapper for data access and Spectre.Console for the UI.

## Requirements coverage
- **Dapper ORM** for all data access (repositories).
- **Spectre.Console** menus, prompts, and tables for display.
- **Separate classes**: `CodingController`, `UserInput`, `Validation`, services, and repositories.
- **Strict datetime format**: `yyyy-MM-dd HH:mm`; validated on input.
- **Duration computed** automatically from start/end; not user-entered.
- **Manual start/end entry** enforced; both required.
- **Configuration** via `appsettings.json` for database settings.

## Running
1. Ensure SQL Server is accessible per `KelvinTawiah.CodingTracker/appsettings.json`.
2. From `KelvinTawiah.CodingTracker/`: `dotnet run`
3. Follow the Spectre.Console menu to add or manage sessions.

## Notes
- Database/tables are created on first run (with duration and logging columns).
- Session logs capture create/update/delete actions with timestamps for auditability.
