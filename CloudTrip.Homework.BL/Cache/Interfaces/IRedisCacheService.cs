using System;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.BL.Cache.Interfaces;

public interface IRedisCacheService
{
    Task<IReadOnlyCollection<AvailableFlight>?> GetCachedFlightsAsync(SearchCriteria criteria);
    Task CacheFlightsAsync(SearchCriteria criteria, IEnumerable<AvailableFlight> flights);
    Task InvalidateCacheAsync(SearchCriteria criteria);
}
