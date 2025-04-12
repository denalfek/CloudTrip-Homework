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
        SearchCriteria criteria,
        SortCriteria sortCriteria,
        CancellationToken ct = default)
    {
        const string cacheKey = "AllVendorsDataKey";

        if (await redisCacheService.GetCachedFlightsAsync(cacheKey) is { } cachedFlights)
        {
            var filtered = FilterResults(criteria, cachedFlights);
            var sorted = SortResults(sortCriteria, filtered);
            return sorted;
        }

        var searchTasks = providers.Select(p => p.Search(ct));

        var taskResults = (await Task.WhenAll(searchTasks))
            .SelectMany(x => x).ToArray();

        _ = Task.Run(() => redisCacheService.CacheFlightsAsync(cacheKey, taskResults), ct);

        var res = SortResults(sortCriteria, FilterResults(criteria, taskResults));
        return res;
    }

    private static AvailableFlight[] SortResults(
        SortCriteria criteria, IReadOnlyCollection<AvailableFlight> flights)
    {
        return criteria.SortField switch
        {
            Common.Dto.SortField.Airline =>
                criteria.Accending ?
                    flights.OrderBy(f => f.Airline).ToArray() :
                    flights.OrderByDescending(f => f.Airline).ToArray(),
            Common.Dto.SortField.DepartureDate =>
                criteria.Accending ?
                    flights.OrderBy(f => f.DepartureDate).ToArray() :
                    flights.OrderByDescending(f => f.DepartureDate).ToArray(),
            Common.Dto.SortField.ArrivalTime =>
                criteria.Accending ?
                    flights.OrderBy(f => f.ArrivalTime).ToArray() :
                    flights.OrderByDescending(f => f.ArrivalTime).ToArray(),
            Common.Dto.SortField.Price =>
                criteria.Accending ?
                    flights.OrderBy(f => f.Price).ToArray() :
                    flights.OrderByDescending(f => f.Price).ToArray(),
            Common.Dto.SortField.Stops =>
                criteria.Accending ?
                    flights.OrderBy(f => f.Stops).ToArray() :
                    flights.OrderByDescending(f => f.Stops).ToArray(),
            _ => flights.ToArray(),
        };
    }

    private static AvailableFlight[] FilterResults(
        SearchCriteria criteria, IReadOnlyCollection<AvailableFlight> flights)
    {
        var query = flights
            .Where(f => f.Airline == criteria.Airline && f.DepartureDate.Date >= criteria.DepartureDate.Date);

        if (query.Count() == 0)
        {
            query = flights
            .Where(f => f.Airline == criteria.Airline);
        }

        if (criteria.MaxStops is not null)
        {
            query = query.Where(f => f.Stops <= criteria.MaxStops);
        }

        if (criteria.MaxPrice is not null)
        {
            query = query.Where(f => f.Price <= criteria.MaxPrice);
        }

        var result = query.ToArray();

        return result;
    }
}
