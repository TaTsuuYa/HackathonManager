using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.User.Dtos;
using HackathonManager.ws.Application.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HackathonManager.ws.Controllers;

[Route("api/users")]
[ApiController]
[Authorize(Roles = RoleNames.Admin)]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _service = userService;

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedDto<GetUserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] FilterUserDto filter)
    {
        PaginatedDto<GetUserDto> users = await _service.GetAllAsync(filter);
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Result<GetUserDto> userResult = await _service.GetUserByIdAsync(id);
        if (!userResult.Success)
            return StatusCode(userResult.StatusCode, userResult.Error);

        return Ok(userResult.Value);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, UpdateUserDto userDto)
    {
        Result<GetUserDto> userResult = await _service.UpdateUserAsync(id, userDto);
        if (!userResult.Success)
            return StatusCode(userResult.StatusCode, userResult.Error);

        return Ok(userResult.Value);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        Result<bool> userResult = await _service.DeleteUserAsync(id);
        if (!userResult.Success)
            return StatusCode(userResult.StatusCode, userResult.Error);

        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(GetUserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(AddUserDto userDto)
    {
        Result<GetUserDto> userResult = await _service.CreateUserAsync(userDto);
        if (!userResult.Success)
            return StatusCode(userResult.StatusCode, userResult.Error);

        return Created($"/api/users/{userResult.Value!.Id}", userResult.Value);
    }

    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([Required] int userId, [Required] string password)
    {
        Result<GetUserDto> userResult = await _service.UpdatePasswordByIdAsync(userId, password);

        if (!userResult.Success)
            return StatusCode(userResult.StatusCode, userResult.Error);

        return Ok("Password Changed Successfully");
    }
}
