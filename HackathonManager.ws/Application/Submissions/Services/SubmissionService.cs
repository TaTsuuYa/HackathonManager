using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.Submissions.Dtos;
using HackathonManager.ws.Domain.Entities;
using HackathonManager.ws.Domain.IRepositories;
using HackathonManager.ws.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HackathonManager.ws.Application.Submissions.Services;

public class SubmissionService(IGenericRepository<Submission, int> repo,
    IGenericRepository<Hackathon, int> hackathonRepo,
    IGenericRepository<Team, int> teamRepo
) : ISubmissionService
{
    private readonly IGenericRepository<Submission, int> _repository = repo;
    private readonly IGenericRepository<Hackathon, int> _hackathonRepository = hackathonRepo;
    private readonly IGenericRepository<Team, int> _teamRepository = teamRepo;

    public async Task<Result<GetSubmissionDto>> GetByIdAsync(int id)
    {
        Submission? submission = await _repository.GetByIdAsync(id);
        if (submission == null)
            return Result<GetSubmissionDto>.Fail("Submission not found", StatusCodes.Status404NotFound);

        return Result<GetSubmissionDto>.Ok(submission.ToDto());
    }

    public async Task<Result<GetSubmissionDto>> CreateAsync(CreateSubmissionDto newSubmission, int userId)
    {
        if (!IsValidUrl(newSubmission.Url))
            return Result<GetSubmissionDto>.Fail("Provided url is not valid url", StatusCodes.Status400BadRequest);

        bool submissionExists = await _repository.GetAll()
            .Where(s => s.HackathonId == newSubmission.HackathonId &&
                s.TeamId == newSubmission.TeamId)
            .AnyAsync();
        if (submissionExists)
            return Result<GetSubmissionDto>.Fail("A submission for this team in this hackathon already exists", StatusCodes.Status400BadRequest);

        Hackathon? hackathon = await GetHackathon(newSubmission.HackathonId);
        if (hackathon is null)
            return Result<GetSubmissionDto>.Fail("Hackathon not found", StatusCodes.Status404NotFound);

        // Check if the hackathon has ended with a 15 minutes wiggle room
        if (hackathon.EndDate.AddMinutes(15) < DateTime.UtcNow)
            return Result<GetSubmissionDto>.Fail("End date already passed", StatusCodes.Status400BadRequest);

        if (hackathon.StartDate > DateTime.UtcNow)
            return Result<GetSubmissionDto>.Fail("This hackathon is not open for submissions yet", StatusCodes.Status400BadRequest);

        Team? team = await GetTeam(newSubmission.TeamId);
        if (team is null)
            return Result<GetSubmissionDto>.Fail("Team not found", StatusCodes.Status404NotFound);

        if (team.LeaderId != userId)
            return Result<GetSubmissionDto>.Fail("Only the leader can submit for this team", StatusCodes.Status403Forbidden);

        Submission submission = new()
        {
            Title = newSubmission.Title,
            Description = newSubmission.Description,
            Url = newSubmission.Url,
            HackathonId = newSubmission.HackathonId,
            TeamId = newSubmission.TeamId
        };

        Submission? createdSubmission = await _repository.CreateAsync(submission);
        if (createdSubmission == null)
            return Result<GetSubmissionDto>.Fail("Failed to create submission", StatusCodes.Status400BadRequest);

        return Result<GetSubmissionDto>.Ok(createdSubmission.ToDto());
    }

    public async Task<Result<bool>> DeleteAsync(int id, int userId)
    {
        Submission? submission = await _repository.GetAll()
            .Include(s => s.Team)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (submission == null)
            return Result<bool>.Fail("Submission not found", StatusCodes.Status404NotFound);

        if (submission.Team!.LeaderId != userId)
            return Result<bool>.Fail("Only the leader can delete this submission", StatusCodes.Status403Forbidden);

        bool isDeleted = await _repository.DeleteAsync(id);
        if (!isDeleted)
            return Result<bool>.Fail("Failed to delete submission", StatusCodes.Status400BadRequest);

        return Result<bool>.Ok(true);
    }

    public async Task<PaginatedDto<GetSubmissionDto>> GetAllAsync(FilterSubmissionDto filter)
    {
        PaginatedDto<Submission> query = await _repository.GetAll()
            .Filter(filter)
            .PaginateAsync(filter);

        return query.RePaginate(s => s.ToDto());
    }

    private async Task<Hackathon?> GetHackathon(int hackathonId)
    {
        Hackathon? hackathon = await _hackathonRepository.GetByIdAsync(hackathonId);
        if (hackathon == null)
            return null;

        return hackathon;
    }

    private async Task<Team?> GetTeam(int teamId)
    {
        Team? team = await _teamRepository.GetByIdAsync(teamId);
        if (team == null)
            return null;

        return team;
    }

    private static bool IsValidUrl(string? url)
        => Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) &&
            (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps) &&
            uri.Host.Contains('.');
}
