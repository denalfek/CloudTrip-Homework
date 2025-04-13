using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.BL.Cache.Interfaces;
using CloudTrip.Homework.BL.Services.Interfaces;
using Microsoft.Extensions.Logging;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.BL.Services;

internal sealed class FlightService(
    IEnumerable<IFlightProvider> providers,
    IRedisCacheService redisCacheService,    
    ILogger<FlightService> logger) : IFlightService
{
    public async Task<IReadOnlyCollection<AvailableFlight>> GetAll(CancellationToken ct = default)
    {
        const string cacheKey = "AllVendorsDataKey";

        if (await redisCacheService.GetCachedFlightsAsync(cacheKey) is { } cachedFlights)
            return cachedFlights;

        var data = await FetchData(ct);
        CacheData(data, ct);
        return data;
    }

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

        var data = await FetchData(ct);
        CacheData(data, ct);
        return data;
    }

    public async Task WarmUpCache(CancellationToken ct = default)
    {
        var data = await FetchData(ct);
        CacheData(data, ct);
    }

    public Task<bool> Book(Book book, CancellationToken ct = default)
    {
        if (!providers.Any(p => p.ProviderName == book.ProviderName))
        {
            logger.LogError($"Data provider: {book.ProviderName} is not supported");
            return Task.FromResult(false);
        }

        var provider = book.ProviderName switch
        {
            "AirFaker" => providers.First(p => p.ProviderName == book.ProviderName),
            "SkyMockVendor" => providers.First(p => p.ProviderName == book.ProviderName),
            _ => throw new NotImplementedException("Somthing went wrong")
        };

        return provider.Book(book.FlightCode, ct);
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

    private async Task<AvailableFlight[]> FetchData(CancellationToken ct = default)
    {
        var searchTasks = providers.Select(p => SafeCallWithRetries(() => p.Search(ct)));
        var taskResults = (await Task.WhenAll(searchTasks))
            .SelectMany(x => x)
            .ToArray();

        return taskResults;
    }

    private void CacheData(AvailableFlight[] data, CancellationToken ct = default)
    {
        const string cacheKey = "AllVendorsDataKey";
        _ = Task.Run(() => redisCacheService.CacheFlightsAsync(cacheKey, data), ct);
    }

    private async Task<T?> SafeCallWithRetries<T>(Func<Task<T>> fetch)
    {
        const int maxRetries = 3;
        int delayBtwnRetriesSec = 1;
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                var result = await fetch();
                return result;
            }
            catch (OperationCanceledException ex)
            {
                logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            var delay = TimeSpan.FromSeconds(delayBtwnRetriesSec);

            await Task.Delay(delay);
            delayBtwnRetriesSec += delayBtwnRetriesSec + attempt;

            logger.LogInformation($"Trying to fetch data. Delay is too big. Current attempt: {attempt}");
        }


        return default;
    }
}
