namespace HackathonManager.ws.Application.Teams.Dtos;

public class UpdateTeamDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int LeaderId { get; set; }
}
