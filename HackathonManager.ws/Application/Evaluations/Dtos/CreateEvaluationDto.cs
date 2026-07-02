using HackathonManager.ws.Application.Constants;
using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Application.Evaluations.Dtos;

public class CreateEvaluationDto
{
    [Range(EvaluationScore.Min, EvaluationScore.Max)]
    public float InnovationScore { get; set; }
    [Range(EvaluationScore.Min, EvaluationScore.Max)]
    public float TechnicalQualityScore { get; set; }
    [Range(EvaluationScore.Min, EvaluationScore.Max)]
    public float PresentationQualityScore { get; set; }
    [Range(EvaluationScore.Min, EvaluationScore.Max)]
    public float SolutionPertinenceScore { get; set; }

    public required int SubmissionId { get; set; }
}
