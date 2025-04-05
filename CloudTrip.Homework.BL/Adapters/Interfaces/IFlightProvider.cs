using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.Bl.Adapters.Interfaces;

// Common interface. This is the wrapper for data providers
public interface IFlightProvider
{
    string ProviderName { get; }

    Task<IReadOnlyCollection<AvailableFlight>> Search(
        SearchCriteria criteria, CancellationToken ct = default);
}
