using KelvinTawiah.CodingTracker.Model;
using KelvinTawiah.CodingTracker.Repository;

namespace KelvinTawiah.CodingTracker.Service;

public class SessionService
{
  private readonly SessionRepository _repository;

  public SessionService(string connectionString)
  {
    _repository = new SessionRepository(connectionString);
  }

  public void AddSession(Session session)
  {
    session.DurationMinutes = (int)(session.EndTime - session.StartTime).TotalMinutes;
    var id = _repository.Create(session);
    session.Id = id;
  }

  public Session? GetSessionById(int id)
  {
    return _repository.FindById(id);
  }

  public void UpdateSession(Session session)
  {
    session.DurationMinutes = (int)(session.EndTime - session.StartTime).TotalMinutes;
    _repository.Update(session);
  }

  public List<Session> GetAllSessions()
  {
    return _repository.GetAll();
  }

  public void DeleteSession(int id)
  {
    _repository.Delete(id);
  }
}