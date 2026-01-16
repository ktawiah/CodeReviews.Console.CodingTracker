namespace KelvinTawiah.CodingTracker.model;

public class Session
{
  public int Id { get; set; }
  public DateTime StartTime { get; set; }
  public DateTime EndTime { get; set; }
  public int DurationMinutes { get; set; }
  public string Notes { get; set; } = string.Empty;
}