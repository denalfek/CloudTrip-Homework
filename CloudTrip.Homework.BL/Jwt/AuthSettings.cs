namespace CloudTrip.Homework.BL.Jwt;

public class AuthSettings
{
    public required string Secret { get; set; }
    public required string TokenIssuer { get; set; }
    public required string Audience { get; set; }

}
