
using CloudTrip.Homework.BL.Services.Interfaces;

namespace CloudTrip.Homework.Server.Orchestration;

public class CacheWarmupService(
    IServiceScopeFactory factory,
    ILogger<CacheWarmupService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken ct)
    {
        logger.LogInformation("Start cache initialization");
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        using var scope = factory.CreateScope();
        var flightService = scope.ServiceProvider.GetRequiredService<IFlightService>();

        await flightService.WarmUpCache(ct);
        logger.LogInformation("Cache initialization successfully finished");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Cache was warmed up successfully");
        return Task.CompletedTask;
    }
}
