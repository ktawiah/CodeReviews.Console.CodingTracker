using KelvinTawiah.CodingTracker.model;
using KelvinTawiah.CodingTracker.repository;

namespace KelvinTawiah.CodingTracker.service;

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