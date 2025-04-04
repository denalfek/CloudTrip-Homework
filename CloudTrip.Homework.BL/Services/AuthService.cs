using CloudTrip.Homework.BL.Repositories.Interfaces;
using CloudTrip.Homework.BL.Services.Interfaces;
using CloudTrip.Homework.Common.Dto;

using BCryptography = BCrypt.Net.BCrypt;

namespace CloudTrip.Homework.BL.Services;

internal sealed class AuthService(
    IUserRepository userRepository,
    IJwtProvider jwtProvider) : IAuthService
{
    public async Task<string?> AuthenticateAsync(string email, string password)
    {
        if (await userRepository.GetByEmailAsync(email) is not { } user) return default;

        if (!BCryptography.Verify(password, user.PasswordHash)) return default;

        var token = jwtProvider.Generate(user);
        return token;
    }

    public async Task RegisterAsync(string email, string password)
    {
        if (await userRepository.Exists(email)) return;

        var hash = BCryptography.HashPassword(password);
        var user = new UserModel { Email = email, PasswordHash = hash };

        await userRepository.AddAsync(user);
    }
}
