namespace AMV.CQRS
{
    public class FindEntityAsyncQuery<TEntity> : IAsyncQuery<TEntity>
    {
        public FindEntityAsyncQuery(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }
    }
}