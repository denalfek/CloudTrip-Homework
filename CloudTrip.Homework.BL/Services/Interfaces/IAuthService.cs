namespace CloudTrip.Homework.BL.Services.Interfaces;

public interface IAuthService
{
    Task<string?> AuthenticateAsync(string email, string password);
    Task RegisterAsync(string email, string password);
}
