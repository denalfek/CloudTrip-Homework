using static CloudTrip.Homework.DataProviders.Contracts.Models.SkyMockModel;

namespace CloudTrip.Homework.DataProviders.Contracts.Services;

public interface ISkyMockVendor
{
    Task<IReadOnlyCollection<SkyMockFlyghtResponse>> FindOptionsAsync(CancellationToken ct = default);
    Task<bool> BookFlight(string id, CancellationToken ct = default);
}
