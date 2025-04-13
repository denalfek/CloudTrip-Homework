namespace CloudTrip.Homework.Dal.Mongo.Context;

internal sealed class MongoDbSettings
{
    public required string DbName { get; set; }

    public required string LogsDbName { get; set; }

    public required string LogsCollectionName { get; set; }

    public string? User { get; set; }

    public string? Password { get; set; }

    public required string Host { get; set; }

    public int? Port { get; set; }

    public string? Params { get; set; }

    public required string Protocol { get; set; }

    public string ConnectionString => BuildConnectionString();

    public string LogsConnectionString => BuildConnectionString(LogsDbName);

    private string BuildConnectionString(string? dbName = null)
    {
        var authPart =
            string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password)
                ? string.Empty
                : $"{User}:{Password}@";
        var db = string.IsNullOrEmpty(dbName) ? string.Empty : $"/{dbName}";
        var port = Port == null ? string.Empty : $":{Port}";
        var parameters = string.IsNullOrEmpty(Params) ? string.Empty : $"/?{Params}";
        var connStr = $"{Protocol}://{authPart}{Host}{port}{db}{parameters}";
        return connStr;
    }
}
