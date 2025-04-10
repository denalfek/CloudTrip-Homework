using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.BL.Cache.Interfaces;
using CloudTrip.Homework.BL.Services.Interfaces;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.BL.Services;

public sealed class FlightService(
    IEnumerable<IFlightProvider> providers,
    IRedisCacheService redisCacheService) : IFlightService
{
    public async Task<IReadOnlyCollection<AvailableFlight>> Search(
        SearchCriteria criteria, CancellationToken ct = default)
    {
        if (await redisCacheService.GetCachedFlightsAsync(criteria) is { } cachedFlights)
        {
            return cachedFlights;
        }

        var searchTasks = providers.Select(p => p.Search(criteria, ct));

        var taskResults = (await Task.WhenAll(searchTasks))
            .SelectMany(x => x)
            .ToArray();

        _ = Task.Run(() => redisCacheService.CacheFlightsAsync(criteria, taskResults), ct);

        return taskResults;
    }

    private Task<IReadOnlyCollection<AvailableFlight> Get()
}
