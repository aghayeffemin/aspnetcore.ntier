using aspnetcore.ntier.DAL.DataContext;
using aspnetcore.ntier.DAL.Entities;
using aspnetcore.ntier.DAL.Repositories.IRepositories;

namespace aspnetcore.ntier.DAL.Repositories;

public class UserRepository(AspNetCoreNTierDbContext aspNetCoreNTierDbContext) : GenericRepository<User>(aspNetCoreNTierDbContext), IUserRepository
{
    public async Task<User> UpdateUserAsync(User user)
    {
        _ = aspNetCoreNTierDbContext.Update(user);

        // Ignore password property update for user
        aspNetCoreNTierDbContext.Entry(user).Property(x => x.Password).IsModified = false;

        await aspNetCoreNTierDbContext.SaveChangesAsync();
        return user;
    }
}
