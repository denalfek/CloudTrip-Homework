using CloudTrip.Homework.BL.Cache.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CloudTrip.Homework.Caching.Redis;

public static class ServiceCollectionExtensions
{
    public static void RegisterCache(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionMultiplexer>(provider =>
            ConnectionMultiplexer.Connect("redis:6679"));

        services.AddScoped<IRedisCacheService, RedisCacheService>();
    }
}
