using CloudTrip.Homework.DataProviders.Contracts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CloudTrip.Homework.Mock.DataProviders;

public static class ServiceCollectionExtensions
{
    public static void RegisterProviders(this IServiceCollection services)
    {
        services.AddSingleton<IAirFakerProvider, AirFakerProvider>();
        services.AddSingleton<ISkyMockVendor, SkyMockVendor>();
    }
}
