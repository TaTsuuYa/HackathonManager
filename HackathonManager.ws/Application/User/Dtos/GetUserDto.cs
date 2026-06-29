namespace HackathonManager.ws.Application.User.Dtos;

public class GetUserDto
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string DisplayName { get; set; }
    public required string Role { get; set; }
}
