using CloudTrip.Homework.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudTrip.Homework.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        await authService.RegisterAsync(request.Email, request.Password);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var token = await authService.AuthenticateAsync(request.Email, request.Password);
        if (token is null)
            return Unauthorized();

        return Ok(new { token });
    }

    public record RegisterRequest(string Email, string Password);
    public record LoginRequest(string Email, string Password);
}
