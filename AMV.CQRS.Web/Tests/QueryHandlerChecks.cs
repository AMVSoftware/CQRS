//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Autofac;
//using Autofac.Core;


//namespace AMV.CQRS.Tests
//{
//    public static class QueryHandlerChecks
//    {
//        public static TestResult CanResolveAllQueryHandlers(IContainer container,
//                                          IEnumerable<Type> typesToScan,
//                                          Type queryDefinition,
//                                          Type handlerDefinition)
//        {
//            var allQueryTypes = typesToScan
//                        .Where(t => t.IsClass && HasGenericInterfaces(t, queryDefinition))
//                        .Where(t => t.BaseType == typeof(object))
//                        .ToList();

//            if (!allQueryTypes.Any())
//            {
//                throw new Exception("No queries are found in the assemblies. Please make sure you pass correct list of assemblies");
//            }

//            var errors = new List<string>();

//            // Act
//            foreach (var queryType in allQueryTypes)
//            {
//                var resultType = queryType.GetGenericInterfaces(queryDefinition)
//                             .First()
//                             .GetGenericArguments()
//                             .Single();

//                var handlerType = handlerDefinition.MakeGenericType(queryType, resultType);
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


//        /// <summary>
//        /// Returns a list of open-generic interfaces on the given type. 
//        /// </summary>
//        /// <param name="type"></param>
//        /// <param name="interface"></param>
//        /// <returns></returns>
//        private static List<Type> GetGenericInterfaces(this Type @type, Type @interface)
//        {
//            var result = type.GetInterfaces()
//                             .Where(t => t.IsGenericType && t.GetGenericTypeDefinition() == @interface)
//                             .ToList();

//            return result;
//        }


//        /// <summary>
//        /// Checks is given type implements open-generic interface
//        /// </summary>
//        /// <param name="this"></param>
//        /// <param name="openGeneric"></param>
//        /// <returns></returns>
//        private static bool HasGenericInterfaces(this Type @this, Type openGeneric)
//        {
//            var result = @this.GetGenericInterfaces(openGeneric).Any();
//            return result;
//        }
//    }
//}
