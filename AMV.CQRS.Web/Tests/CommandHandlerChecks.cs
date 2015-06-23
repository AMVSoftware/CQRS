//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Autofac;
//using Autofac.Core;


//namespace AMV.CQRS.Tests
//{
//    public class CommandHandlerChecks
//    {
//        public static TestResult CanResolveAllHandlers(IContainer container,
//                                          IEnumerable<Type> typesToScan,
//                                          Type commandDefinition,
//                                          Type handlerTypeDefinition)
//        {
//            var allCommandTypes = typesToScan.Where(t => !t.IsAbstract)
//                .Where(t => t.IsClass && commandDefinition.IsAssignableFrom(t))
//                .Where(t => t.BaseType == typeof(object) || !commandDefinition.IsAssignableFrom(t.BaseType))
//                .ToList();

//            if (!allCommandTypes.Any())
//            {
//                throw new Exception("No Commands have been found in the provided types. Please make sure you pass correct list of types to test");
//            }

//            var errors = new List<String>();

//            // Act
//            foreach (var commandType in allCommandTypes)
//            {
//                var handlerType = handlerTypeDefinition.MakeGenericType(commandType);
//                try
//                {
//                    container.Resolve(handlerType);
//                }
//                catch (Exception exception)
//                {
//                    var resolutionException = exception as DependencyResolutionException;
//                    if (resolutionException != null)
//                    {
//                        errors.Add(resolutionException.Message);
//                    }
//                    else
//                    {
//                        errors.Add(exception.ToString());
//                    }
//                }
//            }

//            // Assert
//            var separator = String.Format("{0}{0}------------------------------------{0}", Environment.NewLine);
//            var finalMessage = String.Join(separator, errors);
//            return new TestResult(finalMessage);
//        }
//    }


//    public class TestResult
//    {
//        public TestResult(string errorMessage)
//        {
//            ErrorMessage = errorMessage;
//        }


//        public bool IsSuccess
//        {
//            get { return String.IsNullOrWhiteSpace(ErrorMessage); }
//        }

//        public String ErrorMessage { get; private set; }
//    }
//}
