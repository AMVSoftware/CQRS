using System.Collections.Generic;


namespace AMV.CQRS
{
    public class AllEntitiesAsyncQuery<TEntity> : IAsyncQuery<IEnumerable<TEntity>>
    {
    }
}