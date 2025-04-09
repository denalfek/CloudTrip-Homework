using CloudTrip.Homework.DataProviders.Contracts.Services;
using static CloudTrip.Homework.DataProviders.Contracts.Models.SkyMockModel;

namespace CloudTrip.Homework.Mock.DataProviders;

internal class SkyMockVendor : ISkyMockVendor
{
    private readonly Random _random = new();
    private readonly List<SkyMockFlyghtResponse> _flights;

    public SkyMockVendor()
    {
        _flights = GenerateFlights(20);
    }

    public async Task<IReadOnlyCollection<SkyMockFlyghtResponse>> FindOptionsAsync(
        SkyMockQuery query,
        CancellationToken ct = default)
    {
        var delay = _random.Next(50, 550);

        await Task.Delay(delay, ct);
        return _flights;
    }

    private List<SkyMockFlyghtResponse> GenerateFlights(int count)
    {
        var result = new List<SkyMockFlyghtResponse>();
        var airlineName = "SkyMock";
        for (var i = 0; i < count; i++)
        {
            var departureTime = DateTime.UtcNow.AddDays(_random.Next(1, 10)).AddHours(_random.Next(0, 24));
            var duration = TimeSpan.FromHours(_random.Next(2, 12));
            var arrivalTime = departureTime.Add(duration);
            var price = (_random.NextDouble() * 300 + 50).ToString("F2");
            var stopCount = _random.Next(0, 3);

            result.Add(new SkyMockFlyghtResponse(
                $"SM{_random.Next(100, 999)}",
                airlineName,
                departureTime.ToString("O"),
                arrivalTime.ToString("O"),
                price,
                $"B{_random.Next(1, 30)}",
                _random.Next(0, 2) == 0 ? "Airbus A320" : "Boeing 737",
                stopCount
            ));
        }

        return result;
    }
}
