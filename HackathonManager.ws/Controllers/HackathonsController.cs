using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Hackathons.Dtos;
using HackathonManager.ws.Application.Hackathons.Serives;
using HackathonManager.ws.Application.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackathonManager.ws.Controllers;

[Route("api/hackathons")]
[ApiController]
public class HackathonsController(IHackathonService serice) : ApplicationController
{
    private readonly IHackathonService _service = serice;

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetHackathonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Result<GetHackathonDto> result = await _service.GetByIdAsync(id);
        if (!result.Success)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<GetHackathonDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] FilterHackathonDto filter)
    {
        List<GetHackathonDto> hackathons = await _service.GetAllAsync(filter);
        return Ok(hackathons);
    }

    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    [ProducesResponseType(typeof(GetHackathonDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateHackathonDto newHackathon)
    {
        Result<GetHackathonDto> result = await _service.CreateAsync(newHackathon);
        if (!result.Success)
            return BadRequest(result.Error);

        return Created($"/api/hackathons/{result.Value!.Id}", result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = RoleNames.Admin)]
    [ProducesResponseType(typeof(GetHackathonDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateHackathonDto newHackathon)
    {
        Result<GetHackathonDto> result = await _service.UpdateAsync(id, newHackathon);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = RoleNames.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        Result<bool> result = await _service.DeleteAsync(id);
        if (!result.Success)
            return NotFound(result.Error);

        return NoContent();
    }
}
