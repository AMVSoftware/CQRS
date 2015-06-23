using Microsoft.Practices.ServiceLocation;


namespace AMV.CQRS
{
    public class EventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceLocator container;

        public EventDispatcher(IServiceLocator container)
        {
            this.container = container;
        }


        public void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent
        {
            foreach (var handler in container.GetAllInstances<IDomainEventHandler<TEvent>>())
            {
                handler.Handle(eventToDispatch);
            }
        }
    }
}
