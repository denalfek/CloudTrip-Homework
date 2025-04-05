using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.DataProviders.Contracts.Services;

using static CloudTrip.Homework.Common.Dto.FlightModel;
using static CloudTrip.Homework.DataProviders.Contracts.Models.AirFakeModel;

namespace CloudTrip.Homework.Adapters;

internal sealed class AirFakerAdapter
    (IAirFakerProvider provider): IFlightProvider
{
    public string ProviderName => "AirFaker";

    public async Task<IReadOnlyCollection<AvailableFlight>> Search(
        SearchCriteria criteria,
        CancellationToken ct = default)
    {
        var query = new AirFakeQuery(
            criteria.Origin, criteria.Destination,
            criteria.DepartureDate, criteria.Passengers);

        var providerResponse = await provider.SearchFlightsAsync(query, ct);
        var result = providerResponse
            .Select(r => new AvailableFlight(
                ProviderName, r.FlightCode, r.Carrier,
                r.TakeoffTime, r.LandingTime, r.Price))
            .ToArray();

        return result;
    }
}
