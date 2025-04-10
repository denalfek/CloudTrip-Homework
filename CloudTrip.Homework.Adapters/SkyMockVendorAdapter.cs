using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.DataProviders.Contracts.Services;
using static CloudTrip.Homework.Common.Dto.FlightModel;
using static CloudTrip.Homework.DataProviders.Contracts.Models.SkyMockModel;

namespace CloudTrip.Homework.Adapters;

internal sealed class SkyMockVendorAdapter(
    ISkyMockVendor skyMockVendor) : IFlightProvider
{
    public string ProviderName => "SkyMockVendor";

    public async Task<IReadOnlyCollection<AvailableFlight>> Search(
        SearchCriteria criteria, CancellationToken ct = default)
    {
        var query = new SkyMockQuery(
            criteria.Origin,
            criteria.Destination,
            criteria.DepartureDate.ToString(),
            0);

        var providerResponse = await skyMockVendor.FindOptionsAsync(query, ct);
        var result = providerResponse
            .Select(r => new AvailableFlight(
                ProviderName,
                r.Id,
                r.Airline,
                DateTime.Parse(r.DepartureTime),
                DateTime.Parse(r.ArrivalTime),
                decimal.Parse(r.Cost)))
            .ToArray();

        return result;
    }
}
