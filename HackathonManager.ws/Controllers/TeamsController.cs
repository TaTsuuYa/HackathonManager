using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.Teams.Dtos;
using HackathonManager.ws.Application.Teams.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackathonManager.ws.Controllers;

[Route("api/teams")]
[ApiController]
public class TeamsController(ITeamService service) : ApplicationController
{
    private readonly ITeamService _service = service;

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetTeamDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Result<GetTeamDto> result = await _service.GetByIdAsync(id);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedDto<GetTeamDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] FilterTeamDto filter)
    {
        PaginatedDto<GetTeamDto> hackathons = await _service.GetAllAsync(filter);
        return Ok(hackathons);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Participant)]
    [ProducesResponseType(typeof(GetTeamDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateTeamDto newTeam)
    {
        int userId = GetUserId() ?? 0;
        Result<GetTeamDto> result = await _service.CreateAsync(newTeam, userId);
        if (!result.Success)
            return BadRequest(result.Error);

        return Created($"/api/teams/{result.Value!.Id}", result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleNames.Participant)]
    [ProducesResponseType(typeof(GetTeamDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTeamDto newTeam)
    {
        int userId = GetUserId() ?? 0;
        Result<GetTeamDto> result = await _service.UpdateAsync(id, newTeam, userId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.Error);

        return Ok(result.Value);
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

    [HttpPost("{id:int}/members")]
    [Authorize(Roles = RoleNames.Participant)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Join(int id)
    {
        int userId = GetUserId() ?? 0;
        Result<bool> result = await _service.Join(userId, id);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [HttpDelete("{id:int}/members")]
    [Authorize(Roles = RoleNames.Participant)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Leave(int id)
    {
        int userId = GetUserId() ?? 0;
        Result<bool> result = await _service.Leave(userId, id);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [HttpDelete("{teamId:int}/members/{memberId:int}")]
    [Authorize(Roles = RoleNames.Participant)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Leave(int teamId, int memberId)
    {
        Result<bool> result = await _service.Leave(memberId, teamId);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
