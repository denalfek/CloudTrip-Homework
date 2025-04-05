using static CloudTrip.Homework.DataProviders.Contracts.Models.AirFakeModel;

namespace CloudTrip.Homework.DataProviders.Contracts.Services;

public interface IAirFakerProvider
{
    Task<List<AirFakeResponse>> SearchFlightsAsync(
            AirFakeQuery request,
            CancellationToken ct = default);
}
