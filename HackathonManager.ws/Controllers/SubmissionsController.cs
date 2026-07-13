using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.Submissions.Dtos;
using HackathonManager.ws.Application.Submissions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackathonManager.ws.Controllers;

[Authorize]
[Route("api/submissions")]
[ApiController]
public class SubmissionsController(ISubmissionService serice) : ApplicationController
{
    private readonly ISubmissionService _service = serice;

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetSubmissionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Result<GetSubmissionDto> result = await _service.GetByIdAsync(id);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedDto<GetSubmissionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] FilterSubmissionDto filter)
    {
        PaginatedDto<GetSubmissionDto> submissions = await _service.GetAllAsync(filter);
        return Ok(submissions);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Participant)]
    [ProducesResponseType(typeof(GetSubmissionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateSubmissionDto newSubmission)
    {
        int userId = GetUserId() ?? 0;
        Result<GetSubmissionDto> result = await _service.CreateAsync(newSubmission, userId);
        if (!result.Success)
            return BadRequest(result.Error);

        return Created($"/api/submissions/{result.Value!.Id}", result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleNames.Participant)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = GetUserId() ?? 0;
        Result<bool> result = await _service.DeleteAsync(id, userId);
        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }
}
