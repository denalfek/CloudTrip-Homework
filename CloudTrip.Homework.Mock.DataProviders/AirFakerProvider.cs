using CloudTrip.Homework.DataProviders.Contracts.Services;
using static CloudTrip.Homework.DataProviders.Contracts.Models.AirFakeModel;

namespace CloudTrip.Homework.Mock.DataProviders;

internal sealed class AirFakerProvider : IAirFakerProvider
{
    private readonly Random _random = new();
    private readonly List<AirFakeResponse> _flights;

    public AirFakerProvider()
    {
        _flights = GenerateFakeFlights(20);
    }

    public async Task<List<AirFakeResponse>> SearchFlightsAsync(
        AirFakeQuery request,
        CancellationToken ct = default)
    {        
        var delay = _random.Next(50, 550);

        await Task.Delay(delay, ct);
        return _flights;
    }

    private List<AirFakeResponse> GenerateFakeFlights(int count)
    {
        var result = new List<AirFakeResponse>();
        var baseDate = DateTime.UtcNow.Date.AddDays(1);
        var flightNumbers = Enumerable.Range(100, count)
            .Select(n => $"AF{n}");
        var airlineName = "AirFake";
        foreach (var number in flightNumbers)
        {
            var departureTime = baseDate.AddHours(_random.Next(0, 24));
            var duration = TimeSpan.FromHours(_random.Next(2, 12));
            var arrivalTime = departureTime.Add(duration);
            var price = Math.Round((decimal)(_random.NextDouble() * 300 + 50), 2);
            var stopCount = _random.Next(0, 3);
            result.Add(new AirFakeResponse(
                number, airlineName, departureTime, arrivalTime, price, stopCount));
        }

        return result;
    }
}
