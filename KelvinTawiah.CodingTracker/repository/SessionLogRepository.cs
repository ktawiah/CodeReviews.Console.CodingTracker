using Microsoft.Data.SqlClient;
using Dapper;
using KelvinTawiah.CodingTracker.model;

namespace KelvinTawiah.CodingTracker.repository;

public class SessionLogRepository
{
  private readonly string _connectionString;

  public SessionLogRepository(string connectionString)
  {
    _connectionString = connectionString;
  }

  public void Add(SessionLog log)
  {
    using var connection = new SqlConnection(_connectionString);

    var query = @"
      INSERT INTO SessionLogs (SessionId, Action, Message, LoggedAt)
      VALUES (@SessionId, @Action, @Message, @LoggedAt)";

    connection.Execute(query, log);
  }

  public void Clear()
  {
    using var connection = new SqlConnection(_connectionString);

    var query = "DELETE FROM SessionLogs";

    connection.Execute(query);
  }

  public List<SessionLog> View()
  {
    using var connection = new SqlConnection(_connectionString);

    var query = "SELECT * FROM SessionLogs ORDER BY LoggedAt DESC";

    return connection.Query<SessionLog>(query).ToList();
  }

  public List<SessionLog> FindBy(int id)
  {
    using var connection = new SqlConnection(_connectionString);

    var query = "SELECT * FROM SessionLogs WHERE SessionId = @Id ORDER BY LoggedAt DESC";

    return connection.Query<SessionLog>(query, new { Id = id }).ToList();
  }
}