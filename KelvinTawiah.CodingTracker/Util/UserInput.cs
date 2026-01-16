using KelvinTawiah.CodingTracker.Model;
using Spectre.Console;

namespace KelvinTawiah.CodingTracker;

public static class UserInput
{
  public static Session PromptForSession()
  {
    var start = PromptDate("start time");
    var end = PromptEndDate(start);
    var notes = AnsiConsole.Prompt(
      new TextPrompt<string>("Notes (optional):")
        .AllowEmpty()
    );

    var duration = (int)(end - start).TotalMinutes;

    return new Session
    {
      StartTime = start,
      EndTime = end,
      DurationMinutes = duration,
      Notes = notes
    };
  }

  public static Session PromptForSessionUpdate(Session existing)
  {
    var start = PromptDate("new start time", existing.StartTime);
    var end = PromptEndDate(start, existing.EndTime);
    var notes = AnsiConsole.Prompt(
      new TextPrompt<string>("Notes (optional):")
        .DefaultValue(existing.Notes)
        .AllowEmpty()
    );

    var duration = (int)(end - start).TotalMinutes;

    existing.StartTime = start;
    existing.EndTime = end;
    existing.DurationMinutes = duration;
    existing.Notes = notes;
    return existing;
  }

  public static int PromptForSessionId(string message)
  {
    return AnsiConsole.Prompt(
      new TextPrompt<int>(message)
        .Validate(id => id > 0 ? ValidationResult.Success() : ValidationResult.Error("Id must be positive"))
    );
  }

  private static DateTime PromptDate(string label, DateTime? defaultValue = null)
  {
    while (true)
    {
      var prompt = new TextPrompt<string>($"Enter {label} ({Validation.DateFormat}):")
        .PromptStyle("green");

      if (defaultValue.HasValue)
      {
        prompt = prompt.DefaultValue(defaultValue.Value.ToString(Validation.DateFormat));
      }

      var input = AnsiConsole.Prompt(prompt);

      if (Validation.TryParseDateTime(input, out var dt))
      {
        return dt;
      }

      AnsiConsole.MarkupLine("[red]Invalid format. Please use {0}[/]", Validation.DateFormat);
    }
  }

  private static DateTime PromptEndDate(DateTime start, DateTime? defaultValue = null)
  {
    while (true)
    {
      var end = PromptDate("end time", defaultValue);
      if (Validation.IsEndAfterStart(start, end)) return end;

      AnsiConsole.MarkupLine("[red]End time must be after start time.[/]");
    }
  }
}
