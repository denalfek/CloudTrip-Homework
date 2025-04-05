using CloudTrip.Homework.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightsController(IFlightService service) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<IReadOnlyCollection<AvailableFlight>>> Search()
    {
        var criteria = new SearchCriteria("", "", DateTime.UtcNow, 1);
        var result = await service.Search(criteria);

        return Ok(result);
    }
}
