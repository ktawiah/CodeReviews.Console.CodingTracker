using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KelvinTawiah.CodingTracker.Data;

/// <summary>
/// This factory helps EF Core create a DbContext at design-time (migrations).
/// It reads the connection string from appsettings.json.
/// </summary>
public class CodingTrackerContextFactory : IDesignTimeDbContextFactory<CodingTrackerContext>
{
  public CodingTrackerContext CreateDbContext(string[] args)
  {
    var appSettingsPath = FindPathUpward("appsettings.json");
    var json = File.ReadAllText(appSettingsPath);
    var doc = JsonDocument.Parse(json);

    var dbName = doc.RootElement.GetProperty("Database").GetProperty("Name").GetString();
    var user = doc.RootElement.GetProperty("Database").GetProperty("User").GetString();
    var password = doc.RootElement.GetProperty("Database").GetProperty("Password").GetString();
    var server = doc.RootElement.GetProperty("Database").GetProperty("Server").GetString();

    var connectionString = $"Server={server};Database={dbName};User Id={user};Password={password};TrustServerCertificate=True";

    return new CodingTrackerContext(connectionString);
  }

  private static string FindPathUpward(string fileName)
  {
    var currDir = new DirectoryInfo(Directory.GetCurrentDirectory());
    while (currDir != null)
    {
      var potentialPath = Path.Combine(currDir.FullName, fileName);
      if (File.Exists(potentialPath)) return potentialPath;
      currDir = currDir.Parent;
    }
    throw new FileNotFoundException($"{fileName} not found");
  }
}
