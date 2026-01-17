using KelvinTawiah.CodingTracker.Data;
using KelvinTawiah.CodingTracker.Model;

namespace KelvinTawiah.CodingTracker.Repository;

/// <summary>
/// EF Core repository for SessionLog using LINQ queries.
/// Notice how clean the code is compared to raw SQL strings!
/// </summary>
public class SessionLogRepository
{
  private readonly CodingTrackerContext _context;

  public SessionLogRepository(CodingTrackerContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Add a new log entry.
  /// EF Core generates: INSERT INTO SessionLogs (...)
  /// </summary>
  public void Add(SessionLog log)
  {
    _context.SessionLogs.Add(log);
    _context.SaveChanges();
  }

  /// <summary>
  /// Clear all logs.
  /// EF Core generates: DELETE FROM SessionLogs
  /// </summary>
  public void Clear()
  {
    _context.SessionLogs.RemoveRange(_context.SessionLogs);
    _context.SaveChanges();
  }

  /// <summary>
  /// Get all logs ordered by date using LINQ.
  /// OrderByDescending translates to: ORDER BY LoggedAt DESC
  /// </summary>
  public List<SessionLog> View()
  {
    return _context.SessionLogs
      .OrderByDescending(log => log.LoggedAt)
      .ToList();
  }

  /// <summary>
  /// Find logs by session ID using LINQ Where clause.
  /// Where translates to: WHERE SessionId = @id
  /// </summary>
  public List<SessionLog> FindBy(int id)
  {
    return _context.SessionLogs
      .Where(log => log.SessionId == id)
      .OrderByDescending(log => log.LoggedAt)
      .ToList();
  }
}