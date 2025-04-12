using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.BL.Services.Interfaces;

public interface IFlightService
{
    Task<IReadOnlyCollection<AvailableFlight>> GetAll(CancellationToken ct = default);

    Task<IReadOnlyCollection<AvailableFlight>> Search(
        SearchCriteria criteria,
        SortCriteria sortCriteria,
        CancellationToken ct = default);

    Task<bool> Book(Book book, CancellationToken ct = default);

    Task WarmUpCache(CancellationToken ct = default);
}
