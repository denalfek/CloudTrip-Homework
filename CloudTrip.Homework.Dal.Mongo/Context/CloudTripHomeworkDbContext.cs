using CloudTrip.Homework.Dal.Mongo.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CloudTrip.Homework.Dal.Mongo.Context;

internal sealed class CloudTripHomeworkDbContext(
    IMongoClient client,
    IOptions<MongoDbSettings> options)
{
    private readonly IMongoDatabase _db = client.GetDatabase(options.Value.DbName);

    internal IMongoCollection<User> Users => _db.GetCollection<User>(nameof(Users));
}
