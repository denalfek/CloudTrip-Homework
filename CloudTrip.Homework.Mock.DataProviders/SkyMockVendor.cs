﻿using CloudTrip.Homework.DataProviders.Contracts.Services;
using System.Collections.Immutable;
using static CloudTrip.Homework.DataProviders.Contracts.Models.SkyMockModel;

namespace CloudTrip.Homework.Mock.DataProviders;

internal class SkyMockVendor : ISkyMockVendor
{
    private readonly Random _random = new();
    private readonly ImmutableList<SkyMockFlyghtResponse> _flightsList = [];
    public SkyMockVendor()
    {
        var flights = GenerateFlights(20);
        _flightsList = [..flights];
    }

    public async Task<IReadOnlyCollection<SkyMockFlyghtResponse>> FindOptionsAsync(
        CancellationToken ct = default)
    {
        var delay = _random.Next(700, 1010);

        await Task.Delay(delay, ct);
        return _flightsList;
    }

    public async Task<bool> BookFlight(string id, CancellationToken ct = default)
    {
        var delay = _random.Next(700, 1500);

        await Task.Delay(delay, ct);

        if (_flightsList.Any(f => f.Id == id)) return true;

        return false;
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
            var seatsCount = _random.Next(3, 40);

            result.Add(new SkyMockFlyghtResponse(
                Id: $"SM{_random.Next(100, 999)}",
                Airline: airlineName,
                DepartureTime: departureTime.ToString("O"),
                ArrivalTime: arrivalTime.ToString("O"),
                Cost: price,
                Gate: $"B{_random.Next(1, 30)}",
                PlaneModel: _random.Next(0, 2) == 0 ? "Airbus A320" : "Boeing 737",
                StopCount: stopCount
            ));
        }

        return result;
    }
}
