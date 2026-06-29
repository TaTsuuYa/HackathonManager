namespace HackathonManager.ws.Application.User.Dtos;

public class AddUserDto
{
    public required string Username { get; set; }
    public required string Role { get; set; }
    public required string Password { get; set; }
    public required string DisplayName { get; set; }
}
