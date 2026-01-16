using System.Globalization;

namespace KelvinTawiah.CodingTracker;

public static class Validation
{
  public const string DateFormat = "yyyy-MM-dd HH:mm";

  public static bool TryParseDateTime(string input, out DateTime value)
  {
    return DateTime.TryParseExact(
      input,
      DateFormat,
      CultureInfo.InvariantCulture,
      DateTimeStyles.None,
      out value
    );
  }

  public static bool IsEndAfterStart(DateTime start, DateTime end) => end > start;
}
