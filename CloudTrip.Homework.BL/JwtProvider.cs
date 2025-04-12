using CloudTrip.Homework.Common.Dto;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudTrip.Homework.BL;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly string _secret = "Cloud-trip-client-awessome-secret-key";
    private readonly string _tokenIssuer = "CloudTrip";
    private readonly string _audience = "CloudTripClient";

    public string Generate(UserModel userModel)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userModel.Id),
            new Claim(ClaimTypes.Email, userModel.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenTtl = DateTime.UtcNow.AddHours(1);
        var token = new JwtSecurityToken(
            issuer: _tokenIssuer,
            audience: _audience,
            claims: claims,
            expires: tokenTtl,
            signingCredentials: credentials);

        var result = new JwtSecurityTokenHandler().WriteToken(token)!;

        return result;
    }
}

public interface IJwtProvider
{
    string Generate(UserModel userModel);
}