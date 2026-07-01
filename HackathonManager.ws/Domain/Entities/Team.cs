using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HackathonManager.ws.Domain.Entities;

public class Team
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }

    public required int LeaderId { get; set; }
    [ForeignKey(nameof(LeaderId))]
    [InverseProperty(nameof(AppUser.LeadedTeams))]
    public AppUser? Leader { get; set; }

    [DeleteBehavior(DeleteBehavior.NoAction)]
    public ICollection<AppUser> Members { get; set; } = [];
}
