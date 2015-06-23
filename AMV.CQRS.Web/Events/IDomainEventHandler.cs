namespace AMV.CQRS
{
    public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
    {
        void Handle(TEvent raisedEvent);
    }
}
