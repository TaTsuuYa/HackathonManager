using HackathonManager.ws.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HackathonManager.ws.Domain.IRepositories;

public interface IUserRepository
{
    IQueryable<AppUser> GetAll();
    Task<AppUser?> GetByIdAsync(int id);
    Task<string?> GetRoleAsync(AppUser user);
    Task<AppUser?> CreateUserAsync(AppUser user, string role, string password);
    Task<AppUser?> UpdateUserAsync(int id, AppUser user, string role);
    Task<bool> DeleteUserAsync(AppUser user);
    Task<AppUser?> UpdatePasswordAsync(int id, string newPassword);
    Task<bool?> VerifyPasswordAsync(string username, string password);

    DbSet<IdentityRole<int>> Roles { get; }
    DbSet<IdentityUserRole<int>> UserRoles { get; }
}
