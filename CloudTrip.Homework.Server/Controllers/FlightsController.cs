using CloudTrip.Homework.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class FlightsController(
    IFlightService service,
    ILogger<FlightsController> logger) : ControllerBase
{
    [HttpGet()]
    public async Task<ActionResult<IReadOnlyCollection<AvailableFlight>>> Search(
        [FromQuery] SearchCriteria searchCriteria)
    {
        var criteria = new SearchCriteria("", "", DateTime.UtcNow, 1);
        var result = await service.Search(criteria);

        return Ok(result);
    }
}
