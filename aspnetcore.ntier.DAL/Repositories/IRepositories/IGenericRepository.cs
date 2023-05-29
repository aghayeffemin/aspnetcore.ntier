using System.Linq.Expressions;

namespace aspnetcore.ntier.DAL.Repositories.IRepositories;

public interface IGenericRepository<T> where T : class, new()
{
    Task<T> GetAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<List<T>> AddRangeAsync(List<T> entity);
    Task<List<T>> UpdateRangeAsync(List<T> entity);
}
