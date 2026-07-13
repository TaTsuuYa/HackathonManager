using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Evaluations.Dtos;
using HackathonManager.ws.Application.Evaluations.Services;
using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackathonManager.ws.Controllers;

[Route("api/evaluations")]
[ApiController]
[Authorize]
public class EvaluationsController(IEvaluationService serice) : ApplicationController
{
    private readonly IEvaluationService _service = serice;

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetEvaluationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Result<GetEvaluationDto> result = await _service.GetByIdAsync(id);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedDto<GetEvaluationDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] FilterEvaluationDto filter)
    {
        PaginatedDto<GetEvaluationDto> evaluations = await _service.GetAllAsync(filter);
        return Ok(evaluations);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Mentor)]
    [ProducesResponseType(typeof(GetEvaluationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEvaluationDto newEvaluation)
    {
        int userId = GetUserId() ?? 0;
        Result<GetEvaluationDto> result = await _service.CreateAsync(newEvaluation, userId);
        if (!result.Success)
            return BadRequest(result.Error);

        return Created($"/api/evaluations/{result.Value!.Id}", result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleNames.Mentor)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = GetUserId() ?? 0;
        Result<bool> result = await _service.DeleteAsync(id, userId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
