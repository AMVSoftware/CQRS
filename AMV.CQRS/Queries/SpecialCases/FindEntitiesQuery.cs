using System.Collections.Generic;


namespace AMV.CQRS
{
    public class FindEntitiesQuery<TEntity> : IQuery<IEnumerable<TEntity>>
    {
        public FindEntitiesQuery(IEnumerable<int> ids)
        {
            Ids = ids;
        }

        public IEnumerable<int> Ids { get; private set; }
    }
}