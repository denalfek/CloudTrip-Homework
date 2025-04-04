using CloudTrip.Homework.BL.Repositories.Interfaces;
using CloudTrip.Homework.Dal.Mongo.Context;
using CloudTrip.Homework.Dal.Mongo.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CloudTrip.Homework.Dal.Mongo.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void RegisterRepositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.Configure<MongoDbSettings>(configuration.GetSection(""));
        services.AddTransient<CloudTripHomeworkDbContext>();
        const string connStr = "mongodb://mongo:27017";
        services.AddSingleton<IMongoClient>(provider =>
            new MongoClient(connStr));
        services.AddTransient<IUserRepository, UserRepository>();
    }
}
