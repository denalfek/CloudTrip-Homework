namespace CloudTrip.Homework.Dal.Mongo.Context;

internal sealed class MongoDbSettings
{
    public required string DbName { get; set; } = "CloudTrip";

    public required string User { get; set; }

    public required string Password { get; set; }

    public required string Host { get; set; }

    public required int Port { get; set; }

    public string ConnectionString =>
        $"mongodb://{User}:{Password}@{Host}:{Port}";
}
