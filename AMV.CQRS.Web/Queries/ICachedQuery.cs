using System;


namespace AMV.CQRS
{
    public interface ICachedQuery
    {
        String CacheKey { get; }
        TimeSpan CacheDuration { get; }
    }
}