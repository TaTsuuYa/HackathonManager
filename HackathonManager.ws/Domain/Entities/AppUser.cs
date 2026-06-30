using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonManager.ws.Domain.Entities;

public class AppUser : IdentityUser<int>
{
    public required string DisplayName { get; set; }

    [InverseProperty(nameof(Team.Members))]
    public ICollection<Team> Teams { get; set; } = [];

    [InverseProperty(nameof(Team.Leader))]
    public ICollection<Team> LeadedTeams { get; set; } = [];
}
