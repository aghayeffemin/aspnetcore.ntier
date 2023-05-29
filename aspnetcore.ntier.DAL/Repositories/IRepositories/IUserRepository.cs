using aspnetcore.ntier.DAL.Entities;

namespace aspnetcore.ntier.DAL.Repositories.IRepositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> UpdateUserAsync(User user);
}
