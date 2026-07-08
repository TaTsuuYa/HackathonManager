using HackathonManager.ws.Application.Constants;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.User.Dtos;
using HackathonManager.ws.Application.User.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackathonManager.ws.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController(IUserService service) : ApplicationController
{
    private readonly IUserService _service = service;

    [HttpPost("token")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetToken([FromBody] LoginDto dto)
    {
        Result<string> userResult = await _service.GetToken(dto.Username, dto.Password);
        if (!userResult.Success)
            return StatusCode(userResult.StatusCode, userResult.Error);

        return Ok(userResult.Value);
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        Result<GetUserDto> userResult = await _service.CreateUserAsync(new AddUserDto
        {
            Username = dto.Username,
            DisplayName = dto.DisplayName,
            Password = dto.Password,
            Role = RoleNames.Participant
        });

        if (!userResult.Success)
            return BadRequest(userResult.Error);

        return Ok("User Registered Successfully");
    }

    [Authorize]
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto password)
    {
        int userId = GetUserId() ?? 0;
        Result<GetUserDto> userResult = await _service.ChangePasswordAsync(userId, password.CurrentPassword, password.NewPassword);

        if (!userResult.Success)
            return StatusCode(userResult.StatusCode, userResult.Error);

        return Ok("Password Changed Successfully");
    }
}
