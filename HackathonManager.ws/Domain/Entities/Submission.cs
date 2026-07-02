using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Domain.Entities;

public class Submission
{
    [Key]
    public int Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Url { get; set; }

    public required int TeamId { get; set; }
    public Team? Team { get; set; }

    public required int HackathonId { get; set; }
    public Hackathon? Hackathon { get; set; }

    public ICollection<Evaluation> Evaluations { get; set; } = [];
}
