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

namespace CloudTrip.Homework.Dal.Mongo.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static void RegisterRepositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)));
        services.AddTransient<CloudTripHomeworkDbContext>();
        services.AddSingleton<IMongoClient>(provider =>
        {
            var conf = provider.GetRequiredService<MongoDbSettings>()!;
            return new MongoClient(conf.ConnectionString);
        });
        services.AddTransient<IUserRepository, UserRepository>();
    }
}

public static class LoggingConfigurator
{
    public static void BuildLogger(this WebApplicationBuilder builder)
    {
        var mongoConfig = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!;

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.MongoDBBson(
                mongoConfig.LogsConnectionString,                
                collectionName: mongoConfig.LogsCollectionName,
                restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        builder.Host.UseSerilog();
        EnsureTtlIndex(mongoConfig.LogsConnectionString, mongoConfig.LogsDbName, mongoConfig.LogsCollectionName);
    }

    private static void EnsureTtlIndex(string connStr, string dbName, string collectionName)
    {
        var client = new MongoClient(connStr);
        var db = client.GetDatabase(dbName);
        var indexKeys = Builders<BsonDocument>.IndexKeys.Ascending("UtcTimestamp");
        var expirationSeconds = 60;
        var indexOptions = new CreateIndexOptions { ExpireAfter = TimeSpan.FromSeconds(expirationSeconds) };
        var indexModel = new CreateIndexModel<BsonDocument>(indexKeys, indexOptions);
        var collection = db.GetCollection<BsonDocument>(collectionName);
        collection.Indexes.CreateOne(indexModel);
    }
}

public class RequestLoggingMiddleware(
    RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext ctx)
    {
        logger.LogError("Test error log");

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