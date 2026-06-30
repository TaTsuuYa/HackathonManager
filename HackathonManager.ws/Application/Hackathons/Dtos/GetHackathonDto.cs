namespace HackathonManager.ws.Application.Hackathons.Dtos;

public class GetHackathonDto
{
    public required int Id { get; set; }

    public required string Theme { get; set; }
    public required string Rules { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required string EvaluationCriteria { get; set; }
}
