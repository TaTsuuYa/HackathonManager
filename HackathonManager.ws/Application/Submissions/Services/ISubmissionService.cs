using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.Submissions.Dtos;

namespace HackathonManager.ws.Application.Submissions.Services;

public interface ISubmissionService
{
    Task<PaginatedDto<GetSubmissionDto>> GetAllAsync(FilterSubmissionDto filter);
    Task<Result<GetSubmissionDto>> GetByIdAsync(int id);
    Task<Result<bool>> DeleteAsync(int id, int userId);
    Task<Result<GetSubmissionDto>> CreateAsync(CreateSubmissionDto newSubmission, int userId);
}
