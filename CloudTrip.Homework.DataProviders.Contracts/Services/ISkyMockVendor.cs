using static CloudTrip.Homework.DataProviders.Contracts.Models.SkyMockModel;

namespace CloudTrip.Homework.DataProviders.Contracts.Services;

public interface ISkyMockVendor
{
    Task<IReadOnlyCollection<SkyMockFlyghtResponse>> FindOptionsAsync(
        SkyMockQuery query,
        CancellationToken ct = default);
}
