using System.Threading.Tasks;


namespace AMV.CQRS
{
    public class AsyncCachedQueryHandlerDecorator<TQuery, TResult> : IAsyncQueryHandler<TQuery, TResult> where TQuery : IAsyncQuery<TResult>
    {
        public IAsyncQueryHandler<TQuery, TResult> Decorated { get; set; }
        private readonly ICacheProvider cacheProvider;

        public AsyncCachedQueryHandlerDecorator(IAsyncQueryHandler<TQuery, TResult> decorated, ICacheProvider cacheProvider)
        {
            Decorated = decorated;
            this.cacheProvider = cacheProvider;
        }


        public async Task<TResult> HandleAsync(TQuery query)
        {
            var cachedQuery = query as ICachedQuery;

            if (cachedQuery == null)
            {
                return await Decorated.HandleAsync(query);
            }

            var cachedResult = (TResult)cacheProvider.Get(cachedQuery.CacheKey);

            if (cachedResult == null)
            {
                cachedResult = await Decorated.HandleAsync(query);

                cacheProvider.Set(cachedQuery.CacheKey, cachedResult, cachedQuery.CacheDuration);
            }

            return cachedResult;
        }
    }
}