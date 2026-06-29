using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Domain.Entities;

public class Hackathon
{
    [Key]
    public int Id { get; set; }

    public required string Theme { get; set; }
    public required string Rules { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required string EvaluationCriteria { get; set; }
}
