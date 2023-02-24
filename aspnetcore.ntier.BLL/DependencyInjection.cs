using aspnetcore.ntier.BLL.Services;
using aspnetcore.ntier.BLL.Services.IServices;
using aspnetcore.ntier.BLL.Utilities.AutoMapperProfiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace aspnetcore.ntier.BLL;

public static class DependencyInjection
{
    public static void RegisterBLLDependencies(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddAutoMapper(typeof(AutoMapperProfiles));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
    }
}