using aspnetcore.ntier.DAL.DataContext;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace aspnetcore.ntier.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
{
    private readonly AspNetCoreNTierDbContext _aspNetCoreNTierDbContext;
    public GenericRepository(AspNetCoreNTierDbContext aspNetCoreNTierDbContext)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _aspNetCoreNTierDbContext.AddAsync(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entity)
    {
        await _aspNetCoreNTierDbContext.AddRangeAsync(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<int> DeleteAsync(TEntity entity)
    {
        _ = _aspNetCoreNTierDbContext.Remove(entity);
        return await _aspNetCoreNTierDbContext.SaveChangesAsync();
    }

    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
    {
        return await _aspNetCoreNTierDbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);
    }

    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter = null, CancellationToken cancellationToken = default)
    {
        return await (filter == null ? _aspNetCoreNTierDbContext.Set<TEntity>().ToListAsync(cancellationToken) : _aspNetCoreNTierDbContext.Set<TEntity>().Where(filter).ToListAsync(cancellationToken));
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _ = _aspNetCoreNTierDbContext.Update(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> UpdateRangeAsync(List<TEntity> entity)
    {
        _aspNetCoreNTierDbContext.UpdateRange(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }
}
