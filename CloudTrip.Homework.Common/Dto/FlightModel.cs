namespace CloudTrip.Homework.Common.Dto;

public class FlightModel
{
    public record SearchCriteria(
        string? Origin = null,
        string? Destination = null,
        DateTime? DepartureDate = null,
        int? Passengers = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        int? MaxStops = null,
        string? SortBy = null,
        string? SortOrder = null,
        string? Airline = null);

    public record AvailableFlight(
        string ProviderName,
        string FlightCode,
        string Airline,
        DateTime DepartureTime,
        DateTime ArrivalTime,
        decimal Price);
}
