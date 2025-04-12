using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.BL.Services.Interfaces;

public interface IFlightService
{
    Task<IReadOnlyCollection<AvailableFlight>> Search(
        SearchCriteria criteria,
        SortCriteria sortCriteria,
        CancellationToken ct = default);
}
