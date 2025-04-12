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
    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<AvailableFlight>>> GetAll()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var result = await service.GetAll(cts.Token);
        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<AvailableFlight>>> Search(
        [FromQuery] SearchCriteria searchCriteria,
        [FromQuery] SortCriteria sortCriteria)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var result = await service.Search(searchCriteria, sortCriteria, cts.Token);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Book(
        [FromBody] Book bookRequest)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
            var bookingResult = await service
                .Book(bookRequest, cts.Token);

            return bookingResult ? Ok() : BadRequest();
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
