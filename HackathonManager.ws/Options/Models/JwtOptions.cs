namespace HackathonManager.ws.Options.Models;

public class JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Secret { get; init; }
    public int ExpirationMinutes { get; init; }
}
