using CloudTrip.Homework.DataProviders.Contracts.Services;
using static CloudTrip.Homework.DataProviders.Contracts.Models.AirFakeModel;

namespace CloudTrip.Homework.Mock.DataProviders;

internal sealed class AirFakerProvider : IAirFakerProvider
{
    private readonly Random _random = new();
    public async Task<List<AirFakeResponse>> SearchFlightsAsync(
        AirFakeQuery request,
        CancellationToken ct = default)
    {
        var result = new List<AirFakeResponse>
        {
            new("AF123", "AirFake",
                request.DepartureDate.AddHours(9),
                request.DepartureDate.AddHours(12),
                123.45m)
        };

        var sec = DateTime.UtcNow.Second;
        var delay = _random.Next(0, sec);

        await Task.Delay(delay, ct);
        return result;
    }
}
