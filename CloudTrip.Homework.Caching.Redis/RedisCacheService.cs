using CloudTrip.Homework.BL.Cache.Interfaces;
using Newtonsoft.Json;
using StackExchange.Redis;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.Caching.Redis;

internal sealed class RedisCacheService(IConnectionMultiplexer connectionMultiplexer) : IRedisCacheService
{
    private readonly IDatabase _redisDb = connectionMultiplexer.GetDatabase();

    private static string GetCacheKey(SearchCriteria criteria)
        => $"flights:{criteria.Origin}:{criteria.Destination}:{criteria.DepartureDate:yyyy-MM-dd}:{criteria.Passengers}";

    public async Task CacheFlightsAsync(SearchCriteria criteria, IEnumerable<AvailableFlight> flights)
    {
        if (!flights.Any()) return;

        var key = GetCacheKey(criteria);
        var serialized = JsonConvert.SerializeObject(flights);

        await _redisDb.StringSetAsync(key, serialized);
    }

    public async Task<IReadOnlyCollection<AvailableFlight>?> GetCachedFlightsAsync(SearchCriteria criteria)
    {
        var key = GetCacheKey(criteria);
        var cachedValue = await _redisDb.StringGetAsync(key);

        if (!cachedValue.HasValue) return default;

        var deserialized = JsonConvert.DeserializeObject<AvailableFlight[]>(cachedValue!);

        return deserialized;
    }

    public async Task InvalidateCacheAsync(SearchCriteria criteria)
    {
        var key = GetCacheKey(criteria);
        await _redisDb.KeyDeleteAsync(key);
    }
}
