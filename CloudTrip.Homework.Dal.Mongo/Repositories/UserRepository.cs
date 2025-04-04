using CloudTrip.Homework.Common.Dto;
using CloudTrip.Homework.Dal.Mongo.Context;
using CloudTrip.Homework.Dal.Mongo.Entities;
using CloudTrip.Homework.BL.Repositories.Interfaces;
using MongoDB.Driver;

namespace CloudTrip.Homework.Dal.Mongo.Repositories;

internal sealed class UserRepository(CloudTripHomeworkDbContext ctx) : IUserRepository
{
    private readonly IMongoCollection<User> _collection = ctx.Users;

    public async Task AddAsync(UserModel user)
    {
        var entity = new User { Email = user.Email, PasswordHash = user.PasswordHash,
            CreatedAt = DateTime.UtcNow };

        await _collection.InsertOneAsync(entity);        
    }

    public async Task<bool> Exists(string email)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var result = await _collection.Find(filter).AnyAsync();

        return result;
    }

    public async Task<UserModel?> GetByEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        var entity = await _collection.Find(filter).FirstOrDefaultAsync();
        var model = new UserModel { Id = entity.Id.ToString(), Email = entity.Email, PasswordHash = entity.PasswordHash };
        
        return model;
    }
}
