using CloudTrip.Homework.Common.Dto;

namespace CloudTrip.Homework.BL.Repositories.Interfaces;

public interface IUserRepository
{
    Task<bool> Exists(string email);
    Task<UserModel?> GetByEmailAsync(string email);
    Task AddAsync(UserModel user);
}
