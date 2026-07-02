using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Evaluations.Dtos;
using HackathonManager.ws.Application.Hackathons.Dtos;
using HackathonManager.ws.Application.Submissions.Dtos;
using HackathonManager.ws.Application.Teams.Dtos;
using HackathonManager.ws.Domain.Entities;

namespace HackathonManager.ws.Extensions;

public static class MappingExtensions
{
    public static GetHackathonDto ToDto(this Hackathon hackathon) => new()
    {
        Id = hackathon.Id,
        Theme = hackathon.Theme,
        EvaluationCriteria = hackathon.EvaluationCriteria,
        Rules = hackathon.Rules,
        StartDate = hackathon.StartDate,
        EndDate = hackathon.EndDate
    };

    public static GetTeamDto ToDto(this Team hackathon) => new()
    {
        Id = hackathon.Id,
        Name = hackathon.Name,
        Description = hackathon.Description,
        LeaderId = hackathon.LeaderId,
        LeaderDisplayName = hackathon.Leader?.DisplayName ?? "---",
        Members = hackathon.Members.Select(m => new GetMembersDto()
        {
            Id = m.Id,
            DisplayName = m.DisplayName,
            Role = TeamMemberRoles.GetRole(hackathon.LeaderId, m.Id)
        })
    };

    public static GetSubmissionDto ToDto(this Submission submission) => new()
    {
        Id = submission.Id,
        Title = submission.Title,
        Description = submission.Description,
        Url = submission.Url,
        HackathonId = submission.HackathonId,
        TeamId = submission.TeamId
    };

    public static GetEvaluationDto ToDto(this Evaluation submission) => new()
    {
        Id = submission.Id,
        InnovationScore = submission.InnovationScore,
        PresentationQualityScore = submission.PresentationQualityScore,
        SolutionPertinenceScore = submission.SolutionPertinenceScore,
        TechnicalQualityScore = submission.TechnicalQualityScore,
        SubmissionId = submission.SubmissionId,
        HackathonId = submission.Submission?.HackathonId ?? 0,
        HackathonTheme = submission.Submission?.Hackathon?.Theme ?? "---",
        MentorId = submission.MentorId,
        MentorName = submission.Mentor?.DisplayName ?? "---",
        TeamId = submission.Submission?.TeamId ?? 0,
        TeamName = submission.Submission?.Team?.Name ?? "---",
    };
}
