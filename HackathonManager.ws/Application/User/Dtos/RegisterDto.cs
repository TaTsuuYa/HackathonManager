namespace HackathonManager.ws.Application.User.Dtos;

public class RegisterDto
{
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public required string Password { get; set; }
}
