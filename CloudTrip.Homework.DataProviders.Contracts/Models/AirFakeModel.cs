using System.ComponentModel.DataAnnotations;

namespace CloudTrip.Homework.DataProviders.Contracts.Models;

public class AirFakeModel
{
    public record AirFakeQuery(
       [Required] string Origin,
       [Required] string Destination,
       [Required] DateTime DepartureDate,
       [Required] int PassengerCount);

    public record AirFakeResponse(
        string FlightCode,
        string Carrier,
        DateTime TakeoffTime,
        DateTime LandingTime,
        decimal Price,
        int StopCount);
}
