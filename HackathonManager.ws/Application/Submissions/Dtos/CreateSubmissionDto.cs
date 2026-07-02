using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Application.Submissions.Dtos;

public class CreateSubmissionDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }

    [DataType(DataType.Url)]
    public required string Url { get; set; }

    public required int TeamId { get; set; }
    public required int HackathonId { get; set; }
}
