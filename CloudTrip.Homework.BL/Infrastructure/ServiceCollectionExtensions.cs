using CloudTrip.Homework.BL.Services;
using CloudTrip.Homework.BL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CloudTrip.Homework.BL.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IJwtProvider, JwtProvider>();
        services.AddTransient<IAuthService, AuthService>();
    }
}
