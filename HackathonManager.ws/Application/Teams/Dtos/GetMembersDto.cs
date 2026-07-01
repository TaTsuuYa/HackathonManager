namespace HackathonManager.ws.Application.Teams.Dtos;

public class GetMembersDto
{
    public required int Id { get; set; }
    public required string DisplayName { get; set; }
    public required string Role { get; set; }
}
