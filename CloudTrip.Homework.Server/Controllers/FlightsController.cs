using CloudTrip.Homework.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CloudTrip.Homework.Common.Dto.FlightModel;

namespace CloudTrip.Homework.Server.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class FlightsController(
    IFlightService service,
    ILogger<FlightsController> logger) : ControllerBase
{
    [HttpGet()]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<AvailableFlight>>> Search(
        [FromQuery] SearchCriteria searchCriteria,
        [FromQuery] SortCriteria sortCriteria)
    {
        var result = await service.Search(searchCriteria, sortCriteria);

        return Ok(result);
    }

    [HttpPost]
//    [AllowAnonymous]
    public async Task<IActionResult> Book([FromBody] Book bookRequest)
    {
        try
        {
            var bookingResult = await service
                .Book(bookRequest.ProviderName, bookRequest.FlightCode);

            return bookingResult ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
