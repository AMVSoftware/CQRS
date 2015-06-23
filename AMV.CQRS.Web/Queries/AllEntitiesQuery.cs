using System.Collections.Generic;


namespace AMV.CQRS
{
    public class AllEntitiesQuery<TEntity> : IQuery<IEnumerable<TEntity>>
    {
    }
}
