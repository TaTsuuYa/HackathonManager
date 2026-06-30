using HackathonManager.ws.Application.Hackathons.Dtos;
using HackathonManager.ws.Application.Result;

namespace HackathonManager.ws.Application.Hackathons.Serives;

public interface IHackathonService
{
    Task<List<GetHackathonDto>> GetAllAsync(FilterHackathonDto filter);
    Task<Result<GetHackathonDto>> GetByIdAsync(int id);
    Task<Result<bool>> DeleteAsync(int id);
    Task<Result<GetHackathonDto>> UpdateAsync(int id, UpdateHackathonDto newHackathon);
    Task<Result<GetHackathonDto>> CreateAsync(CreateHackathonDto newHackathon);
}
