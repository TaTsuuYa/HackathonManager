namespace HackathonManager.ws.Application.Submissions.Dtos;

public class UpdateSubmissionDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Url { get; set; }
}
