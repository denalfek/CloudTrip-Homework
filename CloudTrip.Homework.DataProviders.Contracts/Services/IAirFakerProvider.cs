using static CloudTrip.Homework.DataProviders.Contracts.Models.AirFakeModel;

namespace CloudTrip.Homework.DataProviders.Contracts.Services;

public interface IAirFakerProvider
{
    Task<IReadOnlyCollection<AirFakeResponse>> SearchFlightsAsync(CancellationToken ct = default);

    Task<bool> BookFlight(string code, CancellationToken ct = default);
}
