using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.BL.Services.Interfaces;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.BL.Services;

public class FlightService(IEnumerable<IFlightProvider> providers) : IFlightService
{
    public async Task<IReadOnlyCollection<AvailableFlight>> Search(
        SearchCriteria criteria, CancellationToken ct = default)
    {
        var searchTasks = providers.Select(p => p.Search(criteria, ct));
        var taskResults = (await Task.WhenAll(searchTasks))
            .SelectMany(x => x)
            .ToArray();

        return taskResults;
    }
}
