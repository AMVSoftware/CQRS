namespace AMV.CQRS
{
    public interface IDomainEventDispatcher
    {
            void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent;
    }
}
