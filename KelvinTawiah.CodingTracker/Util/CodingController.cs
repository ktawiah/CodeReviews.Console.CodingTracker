using KelvinTawiah.CodingTracker.Model;
using KelvinTawiah.CodingTracker.Service;
using Spectre.Console;

namespace KelvinTawiah.CodingTracker;

public class CodingController
{
  private readonly SessionService _sessionService;
  private readonly SessionLogService _logService;

  public CodingController(SessionService sessionService, SessionLogService logService)
  {
    _sessionService = sessionService;
    _logService = logService;
  }

  public void Run()
  {
    while (true)
    {
      var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
          .Title("[bold blue]Coding Tracker[/]")
          .PageSize(10)
          .AddChoices(
            "Add session",
            "View all sessions",
            "View session details",
            "Update session",
            "Delete session",
            "View logs",
            "Exit")
      );

      switch (choice)
      {
        case "Add session":
          AddSession();
          break;
        case "View all sessions":
          ShowAllSessions();
          break;
        case "View session details":
          ShowSessionDetails();
          break;
        case "Update session":
          UpdateSession();
          break;
        case "Delete session":
          DeleteSession();
          break;
        case "View logs":
          ShowLogs();
          break;
        case "Exit":
          return;
      }
    }
  }

  private void AddSession()
  {
    var session = UserInput.PromptForSession();
    _sessionService.AddSession(session);
    _logService.AddLog(new SessionLog
    {
      SessionId = session.Id,
      Action = "Create",
      Message = "Session added",
      LoggedAt = DateTime.Now
    });

    AnsiConsole.MarkupLine("[green]Session added successfully.[/]");
    WaitForKey();
  }

  private void ShowAllSessions()
  {
    var sessions = _sessionService.GetAllSessions();
    var table = new Table().RoundedBorder();
    table.AddColumn("Id");
    table.AddColumn("Start");
    table.AddColumn("End");
    table.AddColumn("Duration (min)");
    table.AddColumn("Notes");

    foreach (var s in sessions)
    {
      table.AddRow(
        s.Id.ToString(),
        s.StartTime.ToString(Validation.DateFormat),
        s.EndTime.ToString(Validation.DateFormat),
        s.DurationMinutes.ToString(),
        string.IsNullOrWhiteSpace(s.Notes) ? "-" : s.Notes
      );
    }

    if (sessions.Count == 0)
    {
      AnsiConsole.MarkupLine("[yellow]No sessions found.[/]");
    }
    else
    {
      AnsiConsole.Write(table);
    }

    WaitForKey();
  }

  private void ShowSessionDetails()
  {
    var id = UserInput.PromptForSessionId("Enter session Id:");
    var session = _sessionService.GetSessionById(id);
    if (session == null)
    {
      AnsiConsole.MarkupLine("[red]Session not found.[/]");
      WaitForKey();
      return;
    }

    var panel = new Panel($"Id: {session.Id}\nStart: {session.StartTime:yyyy-MM-dd HH:mm}\nEnd: {session.EndTime:yyyy-MM-dd HH:mm}\nDuration (min): {session.DurationMinutes}\nNotes: {session.Notes}")
      .Border(BoxBorder.Rounded)
      .Header("Session Details", Justify.Center);

    AnsiConsole.Write(panel);
    WaitForKey();
  }

  private void UpdateSession()
  {
    var id = UserInput.PromptForSessionId("Enter session Id:");
    var session = _sessionService.GetSessionById(id);
    if (session == null)
    {
      AnsiConsole.MarkupLine("[red]Session not found.[/]");
      WaitForKey();
      return;
    }

    var updated = UserInput.PromptForSessionUpdate(session);
    _sessionService.UpdateSession(updated);
    _logService.AddLog(new SessionLog
    {
      SessionId = updated.Id,
      Action = "Update",
      Message = "Session updated",
      LoggedAt = DateTime.Now
    });

    AnsiConsole.MarkupLine("[green]Session updated.[/]");
    WaitForKey();
  }

  private void DeleteSession()
  {
    var id = UserInput.PromptForSessionId("Enter session Id:");
    _sessionService.DeleteSession(id);
    _logService.AddLog(new SessionLog
    {
      SessionId = id,
      Action = "Delete",
      Message = "Session deleted",
      LoggedAt = DateTime.Now
    });

    AnsiConsole.MarkupLine("[green]Session deleted.[/]");
    WaitForKey();
  }

  private void ShowLogs()
  {
    var logs = _logService.ViewLogs();
    var table = new Table().RoundedBorder();
    table.AddColumn("Id");
    table.AddColumn("SessionId");
    table.AddColumn("Action");
    table.AddColumn("Message");
    table.AddColumn("LoggedAt");

    foreach (var log in logs)
    {
      table.AddRow(
        log.Id.ToString(),
        log.SessionId?.ToString() ?? "-",
        log.Action,
        string.IsNullOrWhiteSpace(log.Message) ? "-" : log.Message,
        log.LoggedAt.ToString(Validation.DateFormat)
      );
    }

    if (logs.Count == 0)
    {
      AnsiConsole.MarkupLine("[yellow]No logs found.[/]");
    }
    else
    {
      AnsiConsole.Write(table);
    }

    WaitForKey();
  }

  private static void WaitForKey()
  {
    AnsiConsole.MarkupLine("\n[grey]Press any key to continue...[/]");
    Console.ReadKey();
  }
}
