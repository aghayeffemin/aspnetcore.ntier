using aspnetcore.ntier.DAL.DataContext;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using aspnetcore.ntier.Entity.Entities;

namespace aspnetcore.ntier.DAL.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private readonly AspNetCoreNTierDbContext _aspNetCoreNTierDbContext;
    public UserRepository(AspNetCoreNTierDbContext aspNetCoreNTierDbContext) : base(aspNetCoreNTierDbContext)
    {
        _aspNetCoreNTierDbContext = aspNetCoreNTierDbContext;
    }
}
