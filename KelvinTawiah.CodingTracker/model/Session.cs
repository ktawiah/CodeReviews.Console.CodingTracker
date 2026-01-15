internal class Session
{
  public int Id { get; private set; }
  public DateTime StartTime { get; private set; }
  public DateTime EndTime { get; private set; }
  public string Notes { get; set; } = string.Empty;
}