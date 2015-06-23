namespace AMV.CQRS
{
    public class CachedQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        public IQueryHandler<TQuery, TResult> Decorated { get; set; }
        private readonly ICacheProvider cacheProvider;

        public CachedQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated, ICacheProvider cacheProvider)
        {
            Decorated = decorated;
            this.cacheProvider = cacheProvider;
        }


        public TResult Handle(TQuery query)
        {
            var cachedQuery = query as ICachedQuery;

            if (cachedQuery == null)
            {
                return Decorated.Handle(query);
            }

            var cacheKey = cachedQuery.CacheKey;
            var cachedObject = cacheProvider.Get(cacheKey);

            if (cachedObject != null && cachedObject is TResult)
            {
                return (TResult)cachedObject;
            }
            
            var cachedResult = Decorated.Handle(query);

            cacheProvider.Set(cachedQuery.CacheKey, cachedResult, cachedQuery.CacheDuration);
            return cachedResult;
        }
    }
}