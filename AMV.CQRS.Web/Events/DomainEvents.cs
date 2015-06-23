namespace AMV.CQRS
{
    public static class DomainEvents
    {
        public static IDomainEventDispatcher Dispatcher { get; set; }

        public static void Raise<TEvent>(TEvent eventToRaise) where TEvent : IDomainEvent
        {
            Dispatcher.Dispatch(eventToRaise);
        }
    }
}