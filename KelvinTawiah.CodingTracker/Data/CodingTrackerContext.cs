using KelvinTawiah.CodingTracker.Model;
using Microsoft.EntityFrameworkCore;

namespace KelvinTawiah.CodingTracker.Data;

/// <summary>
/// DbContext represents your database connection and tracks all entity changes.
/// Think of it as your "session" with the database. It handles:
/// - Mapping models to tables
/// - Tracking changes (what's new, modified, deleted)
/// - Executing queries and saving changes
/// </summary>
public class CodingTrackerContext(string connectionString) : DbContext
{
  /// <summary>
  /// DbSet<T> represents a collection for a specific entity type.
  /// This maps to tables in your database.
  /// </summary>
  public DbSet<Session> Sessions { get; set; }
  public DbSet<SessionLog> SessionLogs { get; set; }

  private readonly string _connectionString = connectionString;

  /// <summary>
  /// Configures the database connection and other options.
  /// This method is called when the context is first created.
  /// </summary>
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    // UseSqlServer tells EF Core to use SQL Server as the database provider
    // TrustServerCertificate=True is needed for local SQL Server instances
    optionsBuilder.UseSqlServer(_connectionString);
  }

  /// <summary>
  /// EF Core automatically reads Data Annotations from your models.
  /// No manual configuration needed for simple scenarios!
  /// </summary>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    // Data Annotations handle all the configuration for us
  }
}
