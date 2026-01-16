using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KelvinTawiah.CodingTracker.Model;

[Table("CodingSessions")]
public class Session
{
  [Key]
  public int Id { get; set; }

  [Required]
  public DateTime StartTime { get; set; }

  [Required]
  public DateTime EndTime { get; set; }

  [Required]
  public int DurationMinutes { get; set; }

  [MaxLength(4000)]
  public string Notes { get; set; } = string.Empty;
}