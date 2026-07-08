using HackathonManager.ws.Application.Constants;

namespace HackathonManager.ws.Application.Hackathons.Dtos;

public class GetHackathonDto
{
    public required int Id { get; set; }

    public required string Theme { get; set; }
    public required string Rules { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public HackathonStatus Status => DateTime.UtcNow switch
    {
        var now when now < StartDate => HackathonStatus.Upcoming,
        var now when now >= StartDate && now <= EndDate => HackathonStatus.Ongoing,
        _ => HackathonStatus.Ended,
    };

    public required string EvaluationCriteria { get; set; }
}
