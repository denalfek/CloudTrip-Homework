using CloudTrip.Homework.Bl.Adapters.Interfaces;
using CloudTrip.Homework.BL.Cache.Interfaces;
using CloudTrip.Homework.BL.Services;
using CloudTrip.Homework.Common.Dto;
using FluentAssertions;
using Moq;
using Microsoft.Extensions.Logging.Abstractions;

namespace CloudTrip.Homework.Tests;

public class FlightServiceTests
{
    private readonly Mock<IRedisCacheService> _cacheMock = new();
    private readonly Mock<IFlightProvider> _providerMock = new();
    private readonly FlightService _service;

    public FlightServiceTests()
    {
        _providerMock.SetupGet(p => p.ProviderName).Returns("AirFaker");
        _providerMock.Setup(p => p.Search(It.IsAny<CancellationToken>()))
            .ReturnsAsync(
            [
                new FlightModel.AvailableFlight(
                    "AirFaker", "AF123", "AirlineA",
                    DateTime.Today.AddDays(1), DateTime.Today.AddDays(1).AddHours(2),
                    150m, 1)
            ]);

        _service = new FlightService(
            [_providerMock.Object],
            _cacheMock.Object,
            NullLogger<FlightService>.Instance
        );
    }

    [Fact]
    public async Task GetAll_ReturnsFromCache_WhenAvailable()
    {
        // Arrange
        var cachedFlights = new[]
        {
            new FlightModel.AvailableFlight("CachedProvider", "C123", "AirlineC", DateTime.Today, DateTime.Today.AddHours(2), 99m, 0)
        };

        _cacheMock.Setup(c => c.GetCachedFlightsAsync(It.IsAny<string>()))
            .ReturnsAsync(cachedFlights);

        // Act
        var result = await _service.GetAll();

        // Assert
        result.Should().BeEquivalentTo(cachedFlights);
        _providerMock.Verify(p => p.Search(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Search_FiltersAndSorts_WhenCacheIsPresent()
    {
        // Arrange
        var flights = new[]
        {
            new FlightModel.AvailableFlight("ProviderA", "A1", "AirlineA", DateTime.Today.AddDays(1), DateTime.Now.AddHours(3), 100, 1),
            new FlightModel.AvailableFlight("ProviderA", "A2", "AirlineA", DateTime.Today.AddDays(2), DateTime.Now.AddHours(5), 200, 0),
            new FlightModel.AvailableFlight("ProviderA", "A3", "AirlineB", DateTime.Today.AddDays(1), DateTime.Now.AddHours(4), 50, 2)
        };

        _cacheMock.Setup(c => c.GetCachedFlightsAsync(It.IsAny<string>()))
            .ReturnsAsync(flights);

        var criteria = new FlightModel.SearchCriteria("AirlineA", DateTime.Today);
        var sort = new FlightModel.SortCriteria(SortField.Price, Accending: true);

        // Act
        var result = await _service.Search(criteria, sort);

        // Assert
        result.Should().HaveCount(2);
        result.First().Price.Should().Be(100);
        result.Last().Price.Should().Be(200);
    }

    [Fact]
    public async Task Book_ReturnsFalse_IfProviderNotSupported()
    {
        // Arrange
        var book = new FlightModel.Book("UnknownProvider", "XX123");

        // Act
        var result = await _service.Book(book);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Book_CallsCorrectProvider()
    {
        // Arrange
        var book = new FlightModel.Book("AirFaker", "AF123");

        _providerMock.Setup(p => p.Book("AF123", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.Book(book);

        // Assert
        result.Should().BeTrue();
        _providerMock.Verify(p => p.Book("AF123", It.IsAny<CancellationToken>()), Times.Once);
    }
}
