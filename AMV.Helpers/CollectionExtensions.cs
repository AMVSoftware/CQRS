using System.Collections.Generic;

namespace AMV.Helpers
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Checks if provided element is not null and adds it to the collection. 
        /// In case the provided value is null, nothing happens
        /// </summary>
        /// <typeparam name="TCollection">Collection Type</typeparam>
        /// <typeparam name="TValue">Generic type of elements stored in the collection</typeparam>
        /// <param name="list">Collection to add to</param>
        /// <param name="value">Value to add to</param>
        /// <returns>Modified collection</returns>
        public static TCollection AddIfNotNull<TCollection, TValue>(this TCollection list, TValue value)
            where TValue : class
            where TCollection : ICollection<TValue>
        {
            if (value != null)
            {
                list.Add(value);
            }
            return list;
        }


        /// <summary>
        /// Adds a range of elements into a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination">Collection to be modified</param>
        /// <param name="source">Elements that will be added to the collection</param>
        /// <returns>The modified collection</returns>
        public static ICollection<T> AddRange<T>(this ICollection<T> destination,
                               IEnumerable<T> source)
        {
            foreach (T item in source)
            {
                destination.Add(item);
            }
            return destination;
        }
    }
}
