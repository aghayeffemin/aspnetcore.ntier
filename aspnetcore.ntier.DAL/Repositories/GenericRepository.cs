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

    public async Task<TEntity> Add(TEntity entity)
    {
        await _aspNetCoreNTierDbContext.AddAsync(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> AddRange(List<TEntity> entity)
    {
        await _aspNetCoreNTierDbContext.AddRangeAsync(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<int> Delete(TEntity entity)
    {
        _ = _aspNetCoreNTierDbContext.Remove(entity);
        return await _aspNetCoreNTierDbContext.SaveChangesAsync();
    }

    public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter = null)
    {
        return await _aspNetCoreNTierDbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
    }

    public async Task<List<TEntity>> GetList(Expression<Func<TEntity, bool>> filter = null)
    {
        return await (filter == null ? _aspNetCoreNTierDbContext.Set<TEntity>().ToListAsync() : _aspNetCoreNTierDbContext.Set<TEntity>().Where(filter).ToListAsync());
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        _ = _aspNetCoreNTierDbContext.Update(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> UpdateRange(List<TEntity> entity)
    {
        _aspNetCoreNTierDbContext.UpdateRange(entity);
        await _aspNetCoreNTierDbContext.SaveChangesAsync();
        return entity;
    }
}
