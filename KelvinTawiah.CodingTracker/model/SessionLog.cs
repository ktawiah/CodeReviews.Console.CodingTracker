namespace KelvinTawiah.CodingTracker.Model;

public class SessionLog
{
  public int Id { get; set; }
  public int? SessionId { get; set; }
  public string Action { get; set; } = string.Empty;
  public string Message { get; set; } = string.Empty;
  public DateTime LoggedAt { get; set; }
}