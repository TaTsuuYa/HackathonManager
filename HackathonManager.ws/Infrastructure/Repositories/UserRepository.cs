using HackathonManager.ws.Domain.Entities;
using HackathonManager.ws.Domain.IRepositories;
using HackathonManager.ws.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HackathonManager.ws.Infrastructure.Repositories;

public class UserRepository(AppDbContext context,
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole<int>> roleManager
) : IUserRepository
{
    private readonly AppDbContext _context = context;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager = roleManager;

    public DbSet<IdentityRole<int>> Roles => _context.Roles;
    public DbSet<IdentityUserRole<int>> UserRoles => _context.UserRoles;

    public async Task<AppUser?> GetByIdAsync(int id) => await _userManager.FindByIdAsync(id.ToString());

    public IQueryable<AppUser> GetAll() => _userManager.Users;

    public async Task<AppUser?> CreateUserAsync(AppUser user, string role, string password)
    {
        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        IdentityResult result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            return null;

        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
            return null;

        await transaction.CommitAsync();
        return user;
    }

    public async Task<AppUser?> UpdateUserAsync(int id, AppUser user, string role)
    {
        bool roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
            return null;

        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

        AppUser? dbUser = await _userManager.FindByIdAsync(id.ToString());
        if (dbUser == null) return null;

        IdentityResult removeRoleResult = await _userManager.RemoveFromRoleAsync(dbUser, (await GetRoleAsync(dbUser))!);
        if (!removeRoleResult.Succeeded)
            return null;

        IdentityResult addRoleResult = await _userManager.AddToRoleAsync(dbUser, role);
        if (!addRoleResult.Succeeded)
            return null;

        IdentityResult updateResult = await _userManager.UpdateAsync(dbUser);
        if (!updateResult.Succeeded)
            return null;

        await transaction.CommitAsync();
        return dbUser;
    }

    public async Task<AppUser?> UpdatePasswordAsync(int id, string newPassword)
    {
        AppUser? user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return null;

        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
        IdentityResult removePwdResult = await _userManager.RemovePasswordAsync(user);
        if (!removePwdResult.Succeeded)
            return null;

        IdentityResult addPwdResult = await _userManager.AddPasswordAsync(user, newPassword);
        if (!addPwdResult.Succeeded)
            return null;

        await transaction.CommitAsync();

        return user;
    }

    public async Task<bool> DeleteUserAsync(AppUser user)
    {
        IdentityResult resutl = await _userManager.DeleteAsync(user);
        return resutl.Succeeded;
    }

    public async Task<string?> GetRoleAsync(AppUser user)
    {
        IList<string> roles = await _userManager.GetRolesAsync(user);
        return roles.FirstOrDefault();
    }

    public async Task<bool?> VerifyPasswordAsync(string username, string password)
    {
        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
        if (user == null)
            return null;

        return await _userManager.CheckPasswordAsync(user, password);
    }
}
