using CloudTrip.Homework.Bl.Adapters.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CloudTrip.Homework.Adapters;

public static class ServiceCollectionExtensions
{
    public static void RegisterAdapters(this IServiceCollection services)
    {
        services.AddScoped<IFlightProvider, AirFakerAdapter>();
        services.AddScoped<IFlightProvider, SkyMockVendorAdapter>();
    }
}
