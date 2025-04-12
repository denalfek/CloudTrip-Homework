using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.DataProviders.Contracts.Services;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.Adapters;

internal sealed class SkyMockVendorAdapter(
    ISkyMockVendor skyMockVendor) : IFlightProvider
{
    public string ProviderName => "SkyMockVendor";

    public Task<bool> Book(string flightId) => skyMockVendor.BookFlight(flightId);

    public async Task<IReadOnlyCollection<AvailableFlight>> Search(CancellationToken ct = default)
    {
        var providerResponse = await skyMockVendor.FindOptionsAsync(ct);
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
}
