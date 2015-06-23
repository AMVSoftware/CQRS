using System;


namespace AMV.CQRS
{
    public class FindEntityQuery<TEntity> : IQuery<TEntity>
    {
        public FindEntityQuery(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
        public FindEntityQuery(Guid id)
        {
            GuidId = id;
        }

        public Guid GuidId { get; private set; }
    }
}
