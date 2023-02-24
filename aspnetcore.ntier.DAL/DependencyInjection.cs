using aspnetcore.ntier.DAL.DataContext;
using aspnetcore.ntier.DAL.Repositories;
using aspnetcore.ntier.DAL.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace aspnetcore.ntier.DAL;

public static class DependencyInjection
{
    public static void RegisterDALDependencies(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddDbContext<AspNetCoreNTierDbContext>(options =>
        {
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUserRepository, UserRepository>();
    }
}