using CloudTrip.Homework.BL.Cache.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CloudTrip.Homework.Caching.Redis;

public static class ServiceCollectionExtensions
{
    public static void RegisterCache(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(provider =>
            ConnectionMultiplexer.Connect(configuration["Redis:ConnectionString"]!));

        services.AddSingleton<IRedisCacheService, RedisCacheService>();
    }
}
