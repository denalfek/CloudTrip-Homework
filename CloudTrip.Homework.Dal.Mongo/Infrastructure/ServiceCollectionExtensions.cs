using CloudTrip.Homework.BL.Repositories.Interfaces;
using CloudTrip.Homework.Dal.Mongo.Context;
using CloudTrip.Homework.Dal.Mongo.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using Serilog.Events;
using System.Diagnostics;

// TODO: separate it to different classes; remove constans into cfgs
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

public static class LoggingConfigurator
{
    public static void BuildLogger(this WebApplicationBuilder builder)
    {
        const string connStr = "mongodb://mongo:27017/CloudTrip-logs";
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.MongoDBBson(connStr, collectionName: "Logs", restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        
        builder.Host.UseSerilog();
    }

    public static void EnsureTtlIndex()
    {
        const string connStr = "mongodb://mongo:27017/CloudTrip-logs";
        var client = new MongoClient(connStr);
        var db = client.GetDatabase("CloudTrip-logs");
        var indexKeys = Builders<BsonDocument>.IndexKeys.Ascending("UtcTimestamp");
        var expirationSeconds = 60;
        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromSeconds(expirationSeconds) };
        var indexModel = new CreateIndexModel<BsonDocument>(indexKeys, indexOptions);
        var collection = db.GetCollection<BsonDocument>("Logs");
        collection.Indexes.CreateOne(indexModel);
    }
}

public class RequestLoggingMiddleware(
    RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext ctx)
    {
        var sw = Stopwatch.StartNew();

        await next(ctx);
        sw.Stop();        
        logger.LogInformation("HTTP {Method} {Path} responded {StatusCode} in {Elapsed} ms",
            ctx.Request.Method,
            ctx.Request.Path,
            ctx.Response.StatusCode,
            sw.ElapsedMilliseconds);
    }
}