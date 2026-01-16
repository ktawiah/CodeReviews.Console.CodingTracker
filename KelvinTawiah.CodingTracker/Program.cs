using System.Text.Json;
using Microsoft.Data.SqlClient;
using KelvinTawiah.CodingTracker;
using KelvinTawiah.CodingTracker.service;

public class Program
{
  static readonly string APP_SETTING_PATH = FindPathUpward("appsettings.json");
  static readonly string MASTER_DB_USER = "master";
  static string? DATABASE_NAME;
  static string? DATABASE_PASSWORD;
  static string? DATABASE_USER;
  static string? DATABASE_SERVER;
  static string? CONNECTION_STRING;


  private static string FindPathUpward(string fileName)
  {
    string path = fileName;

    DirectoryInfo? currDir = new(Directory.GetCurrentDirectory());

    while (currDir != null)
    {
      string potentialPath = Path.Combine(currDir.FullName, fileName);

      if (File.Exists(potentialPath)) return potentialPath;
      currDir = currDir.Parent;
    }

    return path;
  }

  public static void Main(string[] args)
  {
    var appSettings = LoadAppSettings();
    DATABASE_NAME = appSettings.GetProperty("Database").GetProperty("Name").GetString();
    DATABASE_USER = appSettings.GetProperty("Database").GetProperty("User").GetString();
    DATABASE_PASSWORD = appSettings.GetProperty("Database").GetProperty("Password").GetString();
    DATABASE_SERVER = appSettings.GetProperty("Database").GetProperty("Server").GetString();

    InitializeDatabase();
    CONNECTION_STRING = $"Server={DATABASE_SERVER};Database={DATABASE_NAME};User Id={DATABASE_USER};Password={DATABASE_PASSWORD};TrustServerCertificate=True";

    var sessionService = new SessionService(CONNECTION_STRING!);
    var sessionLogService = new SessionLogService(CONNECTION_STRING!);
    var controller = new CodingController(sessionService, sessionLogService);

    controller.Run();
  }

  private static void InitializeDatabase()
  {
    var masterConnString = $"Server={DATABASE_SERVER};Database={MASTER_DB_USER};User Id={DATABASE_USER};Password={DATABASE_PASSWORD};TrustServerCertificate=True";
    var appConnString = $"Server={DATABASE_SERVER};Database={DATABASE_NAME};User Id={DATABASE_USER};Password={DATABASE_PASSWORD};TrustServerCertificate=True";

    using (var conn = new SqlConnection(masterConnString))
    {
      conn.Open();
      var cmd = conn.CreateCommand();
      cmd.CommandText = $"IF DB_ID('{DATABASE_NAME}') IS NULL CREATE DATABASE {DATABASE_NAME}";
      cmd.ExecuteNonQuery();
    }

    // Create tables
    using (var conn = new SqlConnection(appConnString))
    {
      conn.Open();
      var cmd = conn.CreateCommand();
      cmd.CommandText = @"
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CodingSessions')
        BEGIN
          CREATE TABLE CodingSessions (
            Id INT PRIMARY KEY IDENTITY(1,1),
            StartTime DATETIME NOT NULL,
            EndTime DATETIME NOT NULL,
            DurationMinutes INT NOT NULL,
            Notes NVARCHAR(MAX) NULL
          )
        END
        ELSE IF COL_LENGTH('CodingSessions', 'DurationMinutes') IS NULL
        BEGIN
          ALTER TABLE CodingSessions ADD DurationMinutes INT NOT NULL DEFAULT(0);
        END

        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SessionLogs')
        BEGIN
          CREATE TABLE SessionLogs (
            Id INT PRIMARY KEY IDENTITY(1,1),
            SessionId INT NULL,
            Action NVARCHAR(100) NOT NULL,
            Message NVARCHAR(MAX) NULL,
            LoggedAt DATETIME NOT NULL DEFAULT(GETDATE()),
            FOREIGN KEY (SessionId) REFERENCES CodingSessions(Id)
          )
        END
        ELSE
        BEGIN
          IF COL_LENGTH('SessionLogs', 'SessionId') IS NULL
            ALTER TABLE SessionLogs ADD SessionId INT NULL;
          IF COL_LENGTH('SessionLogs', 'Action') IS NULL
            ALTER TABLE SessionLogs ADD Action NVARCHAR(100) NOT NULL DEFAULT('');
          IF COL_LENGTH('SessionLogs', 'Message') IS NULL
            ALTER TABLE SessionLogs ADD Message NVARCHAR(MAX) NULL;
          IF COL_LENGTH('SessionLogs', 'LoggedAt') IS NULL
            ALTER TABLE SessionLogs ADD LoggedAt DATETIME NOT NULL DEFAULT(GETDATE());
        END";
      cmd.ExecuteNonQuery();
    }
  }

  private static JsonElement LoadAppSettings()
  {
    var json = File.ReadAllText(APP_SETTING_PATH);
    return JsonElement.Parse(json);
  }
}