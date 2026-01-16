using Microsoft.Data.SqlClient;
using Dapper;
using KelvinTawiah.CodingTracker.model;

namespace KelvinTawiah.CodingTracker.repository;

public class SessionRepository
{
  private readonly string _connectionString;

  public SessionRepository(string connectionString)
  {
    _connectionString = connectionString;
  }

  public int Create(Session session)
  {
    using var connection = new SqlConnection(_connectionString);

    var query = @"
      INSERT INTO CodingSessions (StartTime, EndTime, DurationMinutes, Notes)
      OUTPUT INSERTED.Id
      VALUES (@StartTime, @EndTime, @DurationMinutes, @Notes)";

    return connection.ExecuteScalar<int>(query, session);
  }

  public Session? FindById(int id)
  {
    using var connection = new SqlConnection(_connectionString);

    var query = "SELECT * FROM CodingSessions WHERE Id = @Id";

    return connection.QueryFirstOrDefault<Session>(query, new { Id = id });
  }

  public void Update(Session session)
  {
    using var connection = new SqlConnection(_connectionString);

    var query = @"
      UPDATE CodingSessions 
      SET StartTime = @StartTime, 
          EndTime = @EndTime, 
          DurationMinutes = @DurationMinutes,
          Notes = @Notes
      WHERE Id = @Id";

    connection.Execute(query, session);
  }

  public List<Session> GetAll()
  {
    using var connection = new SqlConnection(_connectionString);

    var query = "SELECT * FROM CodingSessions";

    return connection.Query<Session>(query).ToList();
  }

  public void Delete(int id)
  {
    using var connection = new SqlConnection(_connectionString);

    var query = "DELETE FROM CodingSessions WHERE Id = @Id";

    connection.Execute(query, new { Id = id });
  }
}