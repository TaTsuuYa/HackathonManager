using HackathonManager.ws.Application.Hackathons.Dtos;
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
}
