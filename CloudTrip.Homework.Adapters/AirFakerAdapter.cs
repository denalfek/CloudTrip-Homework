using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.DataProviders.Contracts.Services;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.Adapters;

internal sealed class AirFakerAdapter
    (IAirFakerProvider provider) : IFlightProvider
{
    public string ProviderName => "AirFaker";

    public async Task<IReadOnlyCollection<AvailableFlight>> Search(CancellationToken ct = default)
    {
        var providerResponse = await provider.SearchFlightsAsync(ct);
        var result = providerResponse
            .Select(r => new AvailableFlight(
                ProviderName,
                r.FlightCode,
                r.Carrier,
                r.TakeoffTime,
                r.LandingTime,
                r.Price,
                r.StopCount))
            .ToArray();

        return result;
    }

    public Task<bool> Book(string flightId, CancellationToken ct = default)
        => provider.BookFlight(flightId, ct);
}
