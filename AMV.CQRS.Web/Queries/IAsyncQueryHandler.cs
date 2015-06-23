using System.Threading.Tasks;


namespace AMV.CQRS
{
    public interface IAsyncQueryHandler<in TQuery, TResult> where TQuery : IAsyncQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}