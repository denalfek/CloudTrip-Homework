using System.ComponentModel.DataAnnotations;

namespace CloudTrip.Homework.DataProviders.Contracts.Models;

public class SkyMockModel
{
    public record SkyMockQuery(
        [Required] string From,
        [Required] string To,
        [Required] string When,
        int Passengers);
    
    public record SkyMockFlyghtResponse(
        string Id,
        string Airline,
        string DepartureTime,
        string ArrivalTime,
        string Cost,
        string Gate,
        string PlaneModel);
}
