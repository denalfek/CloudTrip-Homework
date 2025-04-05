namespace CloudTrip.Homework.Common.Dto;

public class FlightModel
{
    public record GetAvailableFlightQuery(
        string Origin, string Destination, DateTime DepartureDate);

    public record AvailableFlight(
        string ProviderName,
        string FlightCode,
        string Airline,
        DateTime DepartureTime,
        DateTime ArrivalTime,
        decimal Price);
}
