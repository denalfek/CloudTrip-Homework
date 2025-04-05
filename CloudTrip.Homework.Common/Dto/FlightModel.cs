namespace CloudTrip.Homework.Common.Dto;

public class FlightModel
{
    public record SearchCriteria(
        string Origin, string Destination, DateTime DepartureDate, int Passengers);

    public record AvailableFlight(
        string ProviderName,
        string FlightCode,
        string Airline,
        DateTime DepartureTime,
        DateTime ArrivalTime,
        decimal Price);
}
