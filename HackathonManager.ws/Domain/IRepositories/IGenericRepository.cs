using Microsoft.EntityFrameworkCore.Storage;

namespace HackathonManager.ws.Domain.IRepositories;

public interface IGenericRepository<TEntity, TKey>
{
    Task<TEntity?> GetByIdAsync(TKey id);
    IQueryable<TEntity> GetAll();
    Task<TEntity?> CreateAsync(TEntity entity);
    Task<TEntity?> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TKey id);

    Task<IDbContextTransaction> BeginTransactionAsync();
}
