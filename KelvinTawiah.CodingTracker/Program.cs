using System.Text.Json;
using KelvinTawiah.CodingTracker;
using KelvinTawiah.CodingTracker.Data;
using KelvinTawiah.CodingTracker.Service;
using Microsoft.EntityFrameworkCore;

public class Program
{
  static readonly string APP_SETTING_PATH = FindPathUpward("appsettings.json");
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

    CONNECTION_STRING = $"Server={DATABASE_SERVER};Database={DATABASE_NAME};User Id={DATABASE_USER};Password={DATABASE_PASSWORD};TrustServerCertificate=True";

    // Create DbContext instance
    using var context = new CodingTrackerContext(CONNECTION_STRING!);

    // EF Core automatically creates/migrates the database on first use
    context.Database.Migrate();

    var sessionService = new SessionService(context);
    var sessionLogService = new SessionLogService(context);
    var controller = new CodingController(sessionService, sessionLogService);

    controller.Run();
  }

  private static JsonElement LoadAppSettings()
  {
    var json = File.ReadAllText(APP_SETTING_PATH);
    return JsonElement.Parse(json);
  }
}