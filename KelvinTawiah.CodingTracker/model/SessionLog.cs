using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KelvinTawiah.CodingTracker.Model;

[Table("SessionLogs")]
public class SessionLog
{
  [Key]
  public int Id { get; set; }

  [ForeignKey("Session")]
  public int? SessionId { get; set; }

  [Required]
  [MaxLength(100)]
  public string Action { get; set; } = string.Empty;

  [MaxLength(4000)]
  public string Message { get; set; } = string.Empty;

  [Required]
  public DateTime LoggedAt { get; set; }
}