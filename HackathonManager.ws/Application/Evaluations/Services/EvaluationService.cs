using HackathonManager.ws.Application.Evaluations.Dtos;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Domain.Entities;
using HackathonManager.ws.Domain.IRepositories;
using HackathonManager.ws.Extensions;
using Microsoft.EntityFrameworkCore;

namespace HackathonManager.ws.Application.Evaluations.Services;

public class EvaluationService(IGenericRepository<Evaluation, int> repo,
    IGenericRepository<Submission, int> submissionRepo
) : IEvaluationService
{
    private readonly IGenericRepository<Evaluation, int> _repository = repo;
    private readonly IGenericRepository<Submission, int> _submissionRepository = submissionRepo;

    public async Task<Result<GetEvaluationDto>> GetByIdAsync(int id)
    {
        Evaluation? evaluation = await _repository.GetAll()
            .Include(e => e.Mentor)
            .Include(e => e.Submission)
                .ThenInclude(s => s!.Hackathon)
            .Include(e => e.Submission)
                .ThenInclude(s => s!.Team)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (evaluation == null)
            return Result<GetEvaluationDto>.Fail("Evaluation not found", StatusCodes.Status404NotFound);

        return Result<GetEvaluationDto>.Ok(evaluation.ToDto());
    }

    public async Task<Result<GetEvaluationDto>> CreateAsync(CreateEvaluationDto newEvaluation, int userId)
    {
        Submission? submission = await _submissionRepository.GetAll()
            .Include(s => s.Hackathon)
            .FirstOrDefaultAsync(s => s.Id == newEvaluation.SubmissionId);
        if (submission == null)
            return Result<GetEvaluationDto>.Fail("Submission not found", StatusCodes.Status404NotFound);

        if (submission.Hackathon!.EndDate > DateTime.UtcNow)
            return Result<GetEvaluationDto>.Fail("The hackathon has not ended yet", StatusCodes.Status400BadRequest);

        bool evaluationExists = await _repository.GetAll()
            .AnyAsync(e => e.SubmissionId == newEvaluation.SubmissionId &&
                e.MentorId == userId);
        if (evaluationExists)
            return Result<GetEvaluationDto>.Fail("The mentor already evaluated this submission", StatusCodes.Status400BadRequest);

        Evaluation evaluation = new()
        {
            InnovationScore = newEvaluation.InnovationScore,
            PresentationQualityScore = newEvaluation.PresentationQualityScore,
            SolutionPertinenceScore = newEvaluation.SolutionPertinenceScore,
            TechnicalQualityScore = newEvaluation.TechnicalQualityScore,
            MentorId = userId,
            SubmissionId = newEvaluation.SubmissionId,
        };

        Evaluation? createdEvaluation = await _repository.CreateAsync(evaluation);
        if (createdEvaluation == null)
            return Result<GetEvaluationDto>.Fail("Failed to create Evaluation", StatusCodes.Status400BadRequest);

        return await GetByIdAsync(createdEvaluation.Id);
    }

    public async Task<Result<bool>> DeleteAsync(int id, int userId)
    {
        Evaluation? evaluation = await _repository.GetByIdAsync(id);
        if (evaluation == null)
            return Result<bool>.Fail("Evaluation not found", StatusCodes.Status404NotFound);

        if (evaluation.MentorId != userId)
            return Result<bool>.Fail("You are not authorized to delete this evaluation", StatusCodes.Status403Forbidden);

        bool isDeleted = await _repository.DeleteAsync(id);
        if (!isDeleted)
            return Result<bool>.Fail("Failed to delete Evaluation", StatusCodes.Status400BadRequest);

        return Result<bool>.Ok(true);
    }

    public async Task<List<GetEvaluationDto>> GetAllAsync(FilterEvaluationDto filter)
    {
        IEnumerable<Evaluation> query = await _repository.GetAll()
            .Include(e => e.Mentor)
            .Include(e => e.Submission)
                .ThenInclude(s => s!.Hackathon)
            .Include(e => e.Submission)
                .ThenInclude(s => s!.Team)
            .Filter(filter)
            .ToListAsync();

        return [.. query.Select(h => h.ToDto())];
    }
}
