namespace CloudTrip.Homework.Common.Dto;

public class FlightModel
{
    public record SearchCriteria(
        string Origin,
        string Destination,
        DateTime DepartureDate,
        int? Passengers = null,
        decimal? MinPrice = null,
        decimal? MaxPrice = null,
        int? MaxStops = null,
        string? Airline = null);

    public record SortCriteria(
        SortField SortField, bool Accending = true);

    public record AvailableFlight(
        string ProviderName,
        string FlightCode,
        string Airline,
        DateTime DepartureTime,
        DateTime ArrivalTime,
        decimal Price);
}
