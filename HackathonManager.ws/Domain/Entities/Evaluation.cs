using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Domain.Entities;

public class Evaluation
{
    [Key]
    public int Id { get; set; }

    public float InnovationScore { get; set; }
    public float TechnicalQualityScore { get; set; }
    public float PresentationQualityScore { get; set; }
    public float SolutionPertinenceScore { get; set; }

    public required int SubmissionId { get; set; }
    public Submission? Submission { get; set; }
}
