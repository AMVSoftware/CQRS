//using System.Web.Mvc;
//using AMV.CQRS;
//using Autofac.Integration.Mvc;


//namespace AMV.CQRS
//{
//    public class EventDispatcher : IDomainEventDispatcher
//    {
//        public void Dispatch<TEvent>(TEvent eventToDispatch) where TEvent : IDomainEvent
//        {
//            foreach (var handler in AutofacDependencyResolver.Current.GetServices<IDomainEventHandler<TEvent>>())
//            {
//                handler.Handle(eventToDispatch);
//            }
//        }
//    }
//}
