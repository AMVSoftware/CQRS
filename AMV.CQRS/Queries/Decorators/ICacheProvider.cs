using System;


namespace AMV.CQRS
{
    public interface ICacheProvider
    {
        object Get(string key);
        TResult Get<TResult>(string key) where TResult : class;
        void Set(string key, object data, TimeSpan cacheTime);
        bool IsSet(string key);
        void Invalidate(string key);
        void InvalidateAll();
    }

    public class NullCacheProvider : ICacheProvider
    {
        public object Get(string key)
        {
            return new object();
        }


        public TResult Get<TResult>(string key) where TResult : class
        {
            return default(TResult);
        }


        public void Set(string key, object data, TimeSpan cacheTime)
        {
            // do nothing
        }


        public bool IsSet(string key)
        {
            return false;
        }


        public void Invalidate(string key)
        {
            // do nothing
        }


        public void InvalidateAll()
        {
            // do nothing
        }
    }
}