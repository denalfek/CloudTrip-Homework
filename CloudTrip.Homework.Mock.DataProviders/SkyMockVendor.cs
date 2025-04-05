using CloudTrip.Homework.DataProviders.Contracts.Services;
using static CloudTrip.Homework.DataProviders.Contracts.Models.SkyMockModel;

namespace CloudTrip.Homework.Mock.DataProviders;

internal class SkyMockVendor : ISkyMockVendor
{
    private readonly Random _random = new();

    public async Task<IReadOnlyCollection<SkyMockFlyghtResponse>> FindOptionsAsync(
        SkyMockQuery query,
        CancellationToken ct = default)
    {
        var departure = DateTime.Parse(query.When).AddHours(6);
        var arrival = departure.AddHours(2);

        var result = new List<SkyMockFlyghtResponse>
        {
            new ("SM987", "SkyMock",
                new DateTimeOffset(departure).ToUnixTimeSeconds(),
                new DateTimeOffset(arrival).ToUnixTimeSeconds(),
                98.76, "B12", "Airbus A320"),
        };

        var sec = DateTime.UtcNow.Second;
        var delay = _random.Next(0, sec);

        await Task.Delay(delay, ct);
        return result;
    }
}
