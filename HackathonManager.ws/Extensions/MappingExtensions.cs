using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Hackathons.Dtos;
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
}
