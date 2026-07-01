namespace HackathonManager.ws.Application.Teams.Dtos;

public class GetTeamDto
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }

    public required int LeaderId { get; set; }
    public required string LeaderDisplayName { get; set; }
    public IEnumerable<GetMembersDto> Members { get; set; } = [];
}
