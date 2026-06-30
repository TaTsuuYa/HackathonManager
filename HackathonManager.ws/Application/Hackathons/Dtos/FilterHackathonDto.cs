using HackathonManager.ws.Application.Constants;

namespace HackathonManager.ws.Application.Hackathons.Dtos;

public class FilterHackathonDto
{
    public string? Theme { get; set; }
    public HackathonStatus? Status { get; set; }
}
