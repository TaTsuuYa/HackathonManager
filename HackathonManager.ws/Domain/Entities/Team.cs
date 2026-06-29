using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Domain.Entities;

public class Team
{
    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Description { get; set; }

    public ICollection<AppUser> Users { get; set; } = [];
}
