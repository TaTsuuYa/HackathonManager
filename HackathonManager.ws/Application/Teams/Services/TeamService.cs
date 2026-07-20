using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.Teams.Dtos;
using HackathonManager.ws.Domain.Entities;
using HackathonManager.ws.Domain.IRepositories;
using HackathonManager.ws.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HackathonManager.ws.Application.Teams.Services;

public class TeamService(IGenericRepository<Team, int> teamRepo, IGenericRepository<AppUser, int> userRepo) : ITeamService
{
    private readonly IGenericRepository<Team, int> _teamRepository = teamRepo;
    private readonly IGenericRepository<AppUser, int> _userRepository = userRepo;

    private const string TeamNotFoundMessage = "Team not found";

    public async Task<Result<GetTeamDto>> CreateAsync(CreateTeamDto newTeam, int leaderId)
    {
        AppUser? leader = await _userRepository.GetByIdAsync(leaderId);
        if (leader is null)
            return Result<GetTeamDto>.Fail("Leader not found", StatusCodes.Status404NotFound);

        Team team = new()
        {
            Name = newTeam.Name,
            Description = newTeam.Description,
            LeaderId = leaderId,
            Members = [leader]
        };

        Team? createdTeam = await _teamRepository.CreateAsync(team);
        if (createdTeam == null)
            return Result<GetTeamDto>.Fail("Failed to create team", StatusCodes.Status400BadRequest);

        return await GetByIdAsync(team.Id);
    }

    public async Task<Result<bool>> DeleteAsync(int id, int userId)
    {
        Team? team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
            return Result<bool>.Fail("Team not found", StatusCodes.Status404NotFound);

        if (team.LeaderId != userId)
            return Result<bool>.Fail("Only the team leader can delete the team", StatusCodes.Status403Forbidden);

        bool isDeleted = await _teamRepository.DeleteAsync(id);
        if (!isDeleted)
            return Result<bool>.Fail("Failed to delete team", StatusCodes.Status400BadRequest);

        return Result<bool>.Ok(true);
    }

    public async Task<PaginatedDto<GetTeamDto>> GetAllAsync(FilterTeamDto filter)
    {
        PaginatedDto<Team> query = await _teamRepository.GetAll()
            .Include(t => t.Leader)
            .Include(t => t.Members)
            .Filter(filter)
            .PaginateAsync(filter);

        return query.RePaginate(t => t.ToDto());
    }

    public async Task<Result<GetTeamDto>> GetByIdAsync(int id)
    {
        Team? team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
            return Result<GetTeamDto>.Fail(TeamNotFoundMessage, StatusCodes.Status404NotFound);

        Team dbTeam = await _teamRepository.GetAll()
            .Include(t => t.Leader)
            .Include(t => t.Members)
            .FirstAsync(t => t.Id == id);

        return Result<GetTeamDto>.Ok(dbTeam.ToDto());
    }

    public async Task<Result<GetTeamDto>> UpdateAsync(int id, UpdateTeamDto newTeam, int userId)
    {
        AppUser? leader = await _userRepository.GetByIdAsync(newTeam.LeaderId);
        if (leader is null)
            return Result<GetTeamDto>.Fail("Leader not found", StatusCodes.Status404NotFound);

        Team? team = await _teamRepository.GetAll()
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (team == null)
            return Result<GetTeamDto>.Fail(TeamNotFoundMessage, StatusCodes.Status404NotFound);

        if (team.LeaderId != userId)
            return Result<GetTeamDto>.Fail("Only the team leader can update the team", StatusCodes.Status403Forbidden);

        if (!team.Members.Any(m => m.Id == newTeam.LeaderId))
            return Result<GetTeamDto>.Fail("New leader must be a member of the team", StatusCodes.Status400BadRequest);

        team.Name = newTeam.Name;
        team.Description = newTeam.Description;
        team.LeaderId = newTeam.LeaderId;

        Team? updatedTeam = await _teamRepository.UpdateAsync(team);
        if (updatedTeam == null)
            return Result<GetTeamDto>.Fail("Failed to update team", StatusCodes.Status400BadRequest);

        return await GetByIdAsync(team.Id);
    }

    public async Task<Result<bool>> Join(int participantId, int teamId)
    {
        AppUser? participant = await _userRepository.GetByIdAsync(participantId);
        if (participant is null)
            return Result<bool>.Fail("Participant not found", StatusCodes.Status404NotFound);

        Team? team = await _teamRepository.GetAll()
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == teamId);
        if (team is null)
            return Result<bool>.Fail("Team not found", StatusCodes.Status404NotFound);

        if (team.Members.Any(m => m.Id == participantId))
            return Result<bool>.Fail("Participant is already a member of the team", StatusCodes.Status400BadRequest);

        team.Members.Add(participant);
        Team? updatedTeam = await _teamRepository.UpdateAsync(team);
        if (updatedTeam == null)
            return Result<bool>.Fail("Failed to update team", StatusCodes.Status400BadRequest);

        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> Leave(int participantId, int teamId)
    {
        Team? team = await _teamRepository.GetAll()
            .Include(t => t.Members)
            .FirstOrDefaultAsync(t => t.Id == teamId);
        if (team is null)
            return Result<bool>.Fail(TeamNotFoundMessage, StatusCodes.Status404NotFound);

        if (!team.Members.Any(m => m.Id == participantId))
            return Result<bool>.Fail("Participant is not a member of the team", StatusCodes.Status400BadRequest);

        AppUser participant = team.Members.First(m => m.Id == participantId);
        if (participant.Id == team.LeaderId)
            return Result<bool>.Fail("Leader cannot leave the team", StatusCodes.Status400BadRequest);

        team.Members.Remove(participant);
        Team? updatedTeam = await _teamRepository.UpdateAsync(team);
        if (updatedTeam == null)
            return Result<bool>.Fail("Failed to update team", StatusCodes.Status400BadRequest);

        return Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> Kick(int participantId, int teamId, int userId)
    {
        Team? team = await _teamRepository.GetByIdAsync(teamId);
        if (team is null)
            return Result<bool>.Fail(TeamNotFoundMessage, StatusCodes.Status404NotFound);

        if (team.LeaderId != userId)
            return Result<bool>.Fail("Only the team leader can kick a member", StatusCodes.Status403Forbidden);

        return await Leave(participantId, teamId);
    }
}
