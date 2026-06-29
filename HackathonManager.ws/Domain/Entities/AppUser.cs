using Microsoft.AspNetCore.Identity;

namespace HackathonManager.ws.Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public required string DisplayName { get; set; }

    public ICollection<Team> Teams { get; set; } = [];
}
