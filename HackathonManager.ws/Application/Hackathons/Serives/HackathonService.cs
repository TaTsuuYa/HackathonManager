using HackathonManager.ws.Application.Hackathons.Dtos;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Domain.Entities;
using HackathonManager.ws.Domain.IRepositories;
using HackathonManager.ws.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HackathonManager.ws.Application.Hackathons.Serives;

public class HackathonService(IGenericRepository<Hackathon, int> repo) : IHackathonService
{
    private readonly IGenericRepository<Hackathon, int> _repository = repo;

    public async Task<Result<GetHackathonDto>> GetByIdAsync(int id)
    {
        Hackathon? hackathon = await _repository.GetByIdAsync(id);
        if (hackathon == null)
            return Result<GetHackathonDto>.Fail("Hackathon not found", StatusCodes.Status404NotFound);

        return Result<GetHackathonDto>.Ok(hackathon.ToDto());
    }

    public async Task<Result<GetHackathonDto>> CreateAsync(CreateHackathonDto newHackathon)
    {
        if (newHackathon.EndDate < DateTime.UtcNow)
            return Result<GetHackathonDto>.Fail("End date already passed", StatusCodes.Status400BadRequest);

        bool isValid = IsValidDates(newHackathon.StartDate, newHackathon.EndDate);
        if (!isValid)
            return Result<GetHackathonDto>.Fail("Start date is greater than end date", StatusCodes.Status400BadRequest);

        Hackathon hackathon = new()
        {
            Theme = newHackathon.Theme,
            Rules = newHackathon.Rules,
            StartDate = newHackathon.StartDate,
            EndDate = newHackathon.EndDate,
            EvaluationCriteria = newHackathon.EvaluationCriteria
        };

        Hackathon? createdHackathon = await _repository.CreateAsync(hackathon);
        if (createdHackathon == null)
            return Result<GetHackathonDto>.Fail("Failed to create hackathon", StatusCodes.Status400BadRequest);

        return Result<GetHackathonDto>.Ok(createdHackathon.ToDto());
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        bool isDeleted = await _repository.DeleteAsync(id);
        if (!isDeleted)
            return Result<bool>.Fail("Failed to delete hackathon", StatusCodes.Status400BadRequest);

        return Result<bool>.Ok(true);
    }

    public async Task<List<GetHackathonDto>> GetAllAsync(FilterHackathonDto filter)
    {
        IEnumerable<Hackathon> query = await _repository.GetAll()
            .Filter(filter)
            .ToListAsync();

        return [.. query.Select(h => h.ToDto())];
    }

    public async Task<Result<GetHackathonDto>> UpdateAsync(int id, UpdateHackathonDto newHackathon)
    {
        if (newHackathon.EndDate < DateTime.UtcNow)
            return Result<GetHackathonDto>.Fail("End date already passed", StatusCodes.Status400BadRequest);

        bool isValid = IsValidDates(newHackathon.StartDate, newHackathon.EndDate);
        if (!isValid)
            return Result<GetHackathonDto>.Fail("Start date is greater than end date", StatusCodes.Status400BadRequest);

        Hackathon? hackathon = await _repository.GetByIdAsync(id);
        if (hackathon == null)
            return Result<GetHackathonDto>.Fail("Hackathon not found", StatusCodes.Status404NotFound);

        hackathon.Theme = newHackathon.Theme;
        hackathon.Rules = newHackathon.Rules;
        hackathon.StartDate = newHackathon.StartDate;
        hackathon.EndDate = newHackathon.EndDate;
        hackathon.EvaluationCriteria = newHackathon.EvaluationCriteria;

        Hackathon? updatedHackathon = await _repository.UpdateAsync(hackathon);
        if (updatedHackathon == null)
            return Result<GetHackathonDto>.Fail("Failed to update hackathon", StatusCodes.Status400BadRequest);

        return Result<GetHackathonDto>.Ok(updatedHackathon.ToDto());
    }

    private static bool IsValidDates(DateTime startDate, DateTime endDate) => startDate < endDate;
}
