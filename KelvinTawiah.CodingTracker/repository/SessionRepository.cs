using KelvinTawiah.CodingTracker.Data;
using KelvinTawiah.CodingTracker.Model;

namespace KelvinTawiah.CodingTracker.Repository;

/// <summary>
/// EF Core repository using LINQ queries instead of raw SQL.
/// The DbContext handles all database operations automatically.
/// </summary>
public class SessionRepository
{
  private readonly CodingTrackerContext _context;

  public SessionRepository(CodingTrackerContext context)
  {
    _context = context;
  }

  /// <summary>
  /// Create a new session. EF Core automatically generates INSERT SQL.
  /// SaveChanges() commits the transaction to the database.
  /// </summary>
  public int Create(Session session)
  {
    _context.Sessions.Add(session);
    _context.SaveChanges();
    return session.Id; // EF Core automatically sets the Id after SaveChanges
  }

  /// <summary>
  /// Find by ID using LINQ instead of SQL.
  /// FirstOrDefault returns null if not found.
  /// </summary>
  public Session? FindById(int id)
  {
    return _context.Sessions.FirstOrDefault(s => s.Id == id);
  }

  /// <summary>
  /// Update a session. EF Core tracks changes automatically.
  /// Just modify the entity and call SaveChanges.
  /// </summary>
  public void Update(Session session)
  {
    _context.Sessions.Update(session);
    _context.SaveChanges();
  }

  /// <summary>
  /// Get all sessions using LINQ.
  /// ToList() executes the query and returns results.
  /// </summary>
  public List<Session> GetAll()
  {
    return _context.Sessions.ToList();
  }

  /// <summary>
  /// Delete a session by ID.
  /// Find it first, then remove.
  /// </summary>
  public void Delete(int id)
  {
    var session = FindById(id);
    if (session != null)
    {
      _context.Sessions.Remove(session);
      _context.SaveChanges();
    }
  }
}