using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMV.CQRS;
using Microsoft.Practices.ServiceLocation;
using Xunit;


namespace Tests
{
    class MediatorTests
    {
        [Fact]
        public void ProcessCommand_NoErrors_HandlerExecuted()
        {
            //Arrange
            var stubServiceLocator = new StubServiceLocator();

            var meditaor = new Mediator(stubServiceLocator);


            // Act

            // Assert
            //TODO write these tests!
        }
    }


    public class StubServiceLocator : IServiceLocator
    {
        private readonly Dictionary<Type, object> collection;

        public StubServiceLocator()
        {
            this.collection = new Dictionary<Type, object>();
        }


        public void SetService(Type key, object service)
        {
            collection[key] = service;
        }


        public object GetService(Type serviceType)
        {
            return collection[serviceType];
        }


        public object GetInstance(Type serviceType)
        {
            return GetService(serviceType);
        }


        public object GetInstance(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }


        public TService GetInstance<TService>()
        {
            var serviceType = typeof (TService);
            return (TService)collection[serviceType];
        }


        public TService GetInstance<TService>(string key)
        {
            return GetInstance<TService>();
        }


        public IEnumerable<TService> GetAllInstances<TService>()
        {
            throw new NotImplementedException();
        }
    }
}
