using HackathonManager.ws.Domain.IRepositories;
using HackathonManager.ws.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace HackathonManager.ws.Infrastructure.Repositories;

public class GenericRepository<TEntity, TKey>(AppDbContext context) : IGenericRepository<TEntity, TKey>
    where TEntity : class
{
    private readonly AppDbContext _context = context;

    public async Task<TEntity?> GetByIdAsync(TKey id) => await _context.Set<TEntity>().FindAsync(id);

    public IQueryable<TEntity> GetAll() => _context.Set<TEntity>();

    public async Task<TEntity?> CreateAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(TKey id)
    {
        TEntity? entity = await GetByIdAsync(id);
        if (entity == null)
            return false;

        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TEntity?> UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
        => await _context.Database.BeginTransactionAsync();
}
