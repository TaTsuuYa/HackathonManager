using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.Teams.Dtos;

namespace HackathonManager.ws.Application.Teams.Services;

public interface ITeamService
{
    Task<List<GetTeamDto>> GetAllAsync(FilterTeamDto filter);
    Task<Result<GetTeamDto>> GetByIdAsync(int id);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<GetTeamDto>> UpdateAsync(int id, UpdateTeamDto newTeam);
    Task<Result<GetTeamDto>> CreateAsync(CreateTeamDto newTeam, int leaderId);
    Task<Result<bool>> Join(int participantId, int teamId);
    Task<Result<bool>> Leave(int participantId, int teamId);
}
