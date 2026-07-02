namespace HackathonManager.ws.Application.Evaluations.Dtos;

public class GetEvaluationDto
{
    public int Id { get; set; }

    public float InnovationScore { get; set; }
    public float TechnicalQualityScore { get; set; }
    public float PresentationQualityScore { get; set; }
    public float SolutionPertinenceScore { get; set; }

    public required int SubmissionId { get; set; }

    public required int TeamId { get; set; }
    public required string TeamName { get; set; }

    public required int HackathonId { get; set; }
    public required string HackathonTheme { get; set; }

    public required int MentorId { get; set; }
    public required string MentorName { get; set; }
}
