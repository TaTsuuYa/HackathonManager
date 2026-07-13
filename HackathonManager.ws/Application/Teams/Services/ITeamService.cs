using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.Teams.Dtos;

namespace HackathonManager.ws.Application.Teams.Services;

public interface ITeamService
{
    Task<PaginatedDto<GetTeamDto>> GetAllAsync(FilterTeamDto filter);
    Task<Result<GetTeamDto>> GetByIdAsync(int id);
    Task<Result<bool>> DeleteAsync(int id, int userId);
    Task<Result<GetTeamDto>> UpdateAsync(int id, UpdateTeamDto newTeam, int userId);
    Task<Result<GetTeamDto>> CreateAsync(CreateTeamDto newTeam, int leaderId);
    Task<Result<bool>> Join(int participantId, int teamId);
    Task<Result<bool>> Leave(int participantId, int teamId);
}
