namespace CloudTrip.Homework.Caching.Redis;

internal sealed class CacheSettings
{
    public required string CacheProvider { get; set; }
    public required string DefaultKey { get; set; }
}
