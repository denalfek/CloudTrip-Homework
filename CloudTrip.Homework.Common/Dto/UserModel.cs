namespace CloudTrip.Homework.Common.Dto;

public class UserModel
{
    public string Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
}
