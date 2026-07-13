using HackathonManager.ws.Application.Pagination;
using HackathonManager.ws.Application.Result;
using HackathonManager.ws.Application.User.Dtos;

namespace HackathonManager.ws.Application.User.Services;

public interface IUserService
{
    Task<Result<GetUserDto>> GetUserByIdAsync(int id);
    Task<PaginatedDto<GetUserDto>> GetAllAsync(FilterUserDto filter);
    Task<Result<GetUserDto>> UpdateUserAsync(int id, UpdateUserDto userDto);
    Task<Result<GetUserDto>> CreateUserAsync(AddUserDto userDto);
    Task<Result<bool>> DeleteUserAsync(int id);
    Task<Result<GetUserDto>> UpdatePasswordByIdAsync(int id, string newPassword);
    Task<Result<GetUserDto>> ChangePasswordAsync(int id, string currentPassword, string newPassword);
    Task<Result<string>> GetToken(string username, string password);
}
