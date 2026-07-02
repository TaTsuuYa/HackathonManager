using HackathonManager.ws.Application.Evaluations.Dtos;
using HackathonManager.ws.Application.Result;

namespace HackathonManager.ws.Application.Evaluations.Services;

public interface IEvaluationService
{
    Task<List<GetEvaluationDto>> GetAllAsync(FilterEvaluationDto filter);
    Task<Result<GetEvaluationDto>> GetByIdAsync(int id);
    Task<Result<bool>> DeleteAsync(int id, int userId);
    Task<Result<GetEvaluationDto>> CreateAsync(CreateEvaluationDto newEvaluation, int userId);
}
