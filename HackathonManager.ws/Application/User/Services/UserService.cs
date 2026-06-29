using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.User.Dtos;
using HackathonManager.ws.Domain.Entities;
using HackathonManager.ws.Domain.IRepositories;
using HackathonManager.ws.Options.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HackathonManager.ws.Application.User.Services;

public class UserService(IUserRepository userRepository, IOptions<JwtOptions> JwtOptions) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly JwtOptions _jwtOptions = JwtOptions.Value;

    private const string UserNotFoundMessage = "User not found";

    public async Task<Result<GetUserDto>> GetUserByIdAsync(int id)
    {
        AppUser? dbUser = await _userRepository.GetByIdAsync(id);
        if (dbUser == null)
            return Result<GetUserDto>.Fail(UserNotFoundMessage, StatusCodes.Status404NotFound);

        string? role = await _userRepository.GetRoleAsync(dbUser);
        GetUserDto user = new()
        {
            Id = dbUser.Id,
            UserName = dbUser.UserName!,
            DisplayName = dbUser.DisplayName,
            Role = role!,
        };

        return Result<GetUserDto>.Ok(user);
    }

    public async Task<Result<bool>> DeleteUserAsync(int id)
    {
        AppUser? user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return Result<bool>.Fail(UserNotFoundMessage, StatusCodes.Status404NotFound);

        return Result<bool>.Ok(await _userRepository.DeleteUserAsync(user));
    }

    public async Task<Result<GetUserDto>> CreateUserAsync(AddUserDto userDto)
    {
        AppUser user = new()
        {
            UserName = userDto.Username,
            DisplayName = userDto.DisplayName,
        };

        AppUser? createdUser = await _userRepository.CreateUserAsync(user, userDto.Role, userDto.Password);
        if (createdUser == null)
            return Result<GetUserDto>.Fail("Failed to create user", StatusCodes.Status400BadRequest);

        return await GetUserByIdAsync(createdUser.Id);
    }

    public async Task<List<GetUserDto>> GetAll(FilterUserDto filter)
    {
        List<AppUser> users = await _userRepository.GetAll().ToListAsync();
        List<GetUserDto> mappedUsers = [.. users.Select(u => new GetUserDto()
        {
            Id = u.Id,
            DisplayName = u.DisplayName,
            UserName = u.UserName!,
            Role = _userRepository.GetRoleAsync(u).Result!,
        })];

        if (filter.Name != null)
        {
            mappedUsers = [.. mappedUsers.Where(u => u.UserName!.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase) ||
                u.DisplayName!.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase))];
        }

        if (filter.Role != null)
            mappedUsers = [.. mappedUsers.Where(u => u.Role.Equals(filter.Role, StringComparison.InvariantCultureIgnoreCase))];

        return mappedUsers;
    }

    public async Task<Result<GetUserDto>> UpdatePasswordByIdAsync(int id, string newPassword)
    {
        AppUser? user = await _userRepository.UpdatePasswordAsync(id, newPassword);
        if (user == null)
            return Result<GetUserDto>.Fail("Failed to update password", StatusCodes.Status400BadRequest);

        return await GetUserByIdAsync(id);
    }

    public async Task<Result<GetUserDto>> ChangePasswordAsync(int id, string currentPassword, string newPassword)
    {
        AppUser? dbUser = await _userRepository.GetByIdAsync(id);
        if (dbUser == null)
            return Result<GetUserDto>.Fail(UserNotFoundMessage, StatusCodes.Status404NotFound);

        bool? isValidPassword = await _userRepository.VerifyPasswordAsync(dbUser.UserName!, currentPassword);
        if (isValidPassword != true)
            return Result<GetUserDto>.Fail("Wrong password", StatusCodes.Status400BadRequest);

        AppUser? user = await _userRepository.UpdatePasswordAsync(id, newPassword);
        if (user == null)
            return Result<GetUserDto>.Fail("Failed to update password", StatusCodes.Status400BadRequest);

        return await GetUserByIdAsync(id);
    }

    public async Task<Result<GetUserDto>> UpdateUserAsync(int id, UpdateUserDto userDto)
    {
        AppUser? existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return Result<GetUserDto>.Fail(UserNotFoundMessage, StatusCodes.Status404NotFound);

        existingUser.DisplayName = userDto.DisplayName;

        AppUser? user = await _userRepository.UpdateUserAsync(id, existingUser, userDto.Role);
        if (user == null)
            return Result<GetUserDto>.Fail("Failed to update user", StatusCodes.Status400BadRequest);

        return await GetUserByIdAsync(id);
    }

    public async Task<Result<string>> GetToken(string username, string password)
    {
        bool? isValid = await _userRepository.VerifyPasswordAsync(username, password);
        if (!isValid.HasValue || !isValid.Value)
            return Result<string>.Fail("Invalid credentials", StatusCodes.Status401Unauthorized);

        AppUser? user = await _userRepository.GetAll()
            .Where(u => u.UserName == username)
            .FirstOrDefaultAsync();
        if (user == null)
            return Result<string>.Fail("Invalid credentials", StatusCodes.Status401Unauthorized);

        GetUserDto userDto = new()
        {
            Id = user.Id,
            UserName = user.UserName!,
            DisplayName = user.DisplayName,
            Role = (await _userRepository.GetRoleAsync(user))!,
        };

        string token = GenerateToken(userDto);

        return Result<string>.Ok(token);
    }

    private string GenerateToken(GetUserDto user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Secret);
        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role)
        ];

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
