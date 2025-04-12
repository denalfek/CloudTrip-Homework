using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.DataProviders.Contracts.Services;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.Adapters;

internal sealed class SkyMockVendorAdapter(
    ISkyMockVendor provider) : IFlightProvider
{
    public string ProviderName => "SkyMockVendor";

    public async Task<IReadOnlyCollection<AvailableFlight>> Search(CancellationToken ct = default)
    {
        var providerResponse = await provider.FindOptionsAsync(ct);
        var result = providerResponse
            .Select(r => new AvailableFlight(
                ProviderName,
                r.Id,
                r.Airline,
                DateTime.Parse(r.DepartureTime),
                DateTime.Parse(r.ArrivalTime),
                decimal.Parse(r.Cost),
                r.StopCount))
            .ToArray();

        return result;
    }

    public Task<bool> Book(string flightId, CancellationToken ct = default)
        => provider.BookFlight(flightId, ct);
}
