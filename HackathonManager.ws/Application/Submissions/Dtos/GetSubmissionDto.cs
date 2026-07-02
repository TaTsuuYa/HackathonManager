namespace HackathonManager.ws.Application.Submissions.Dtos;

public class GetSubmissionDto
{
    public int Id { get; set; }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Url { get; set; }

    public required int TeamId { get; set; }

    public required int HackathonId { get; set; }
}
