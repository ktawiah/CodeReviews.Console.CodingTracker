using KelvinTawiah.CodingTracker.Model;
using KelvinTawiah.CodingTracker.Repository;

namespace KelvinTawiah.CodingTracker.Service;

public class SessionLogService
{
  private readonly SessionLogRepository _repository;

  public SessionLogService(string connectionString)
  {
    _repository = new SessionLogRepository(connectionString);
  }

  public void AddLog(SessionLog log)
  {
    _repository.Add(log);
  }

  public void ClearLog()
  {
    _repository.Clear();
  }

  public List<SessionLog> ViewLogs()
  {
    return _repository.View();
  }

  public List<SessionLog> ViewLogsFor(int id)
  {
    return _repository.FindBy(id);
  }
}