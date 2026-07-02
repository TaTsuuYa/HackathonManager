namespace HackathonManager.ws.Application.Submissions.Dtos;

public class FilterSubmissionDto
{

    public string? Query { get; set; }

    public int? TeamId { get; set; }
    public int? HackathonId { get; set; }
}
