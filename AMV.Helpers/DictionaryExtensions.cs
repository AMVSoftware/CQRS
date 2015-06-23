using System;
using System.Collections.Generic;

namespace AMV.Helpers
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Merges string values in dictionary. Used mostly in merging css classes for html elements.
        /// If there is a value for provided key already in the dictionary, value is added to end of the existing value, space-separated.
        /// If there is no value for provided key, a new key-value pair is created
        /// </summary>
        /// <param name="dictionary">Dictionary to be modified</param>
        /// <param name="key">Key for the pair</param>
        /// <param name="value">Value to be merged into</param>
        /// <returns>Modified dictionary</returns>
        public static IDictionary<string, object> Merge(this IDictionary<string, object> dictionary, String key, String value)
        {
            object result;
            if (!dictionary.TryGetValue(key, out result))
            {
                dictionary.Add(key, value);
                return dictionary;
            }
            var existingValue = result as String;
            if (existingValue == null)
            {
                return dictionary;
            }

            var newValue = existingValue + " " + value;
            dictionary.Remove(key);
            dictionary.Add(key, newValue);
            return dictionary;
        }


        /// <summary>
        /// Checks if dictionary has the key and replaces existing value. 
        /// And if the key does not exist, adds new value
        /// </summary>
        /// <param name="dictionary">Dictionary to be modified</param>
        /// <param name="key">Key for the pair</param>
        /// <param name="value">Value to be added</param>
        /// <returns>Modified dictionary</returns>
        public static IDictionary<String, String> AddOrReplace(this IDictionary<String, String> dictionary, String key, String value)
        {
            String result;
            if (!dictionary.TryGetValue(key, out result))
            {
                dictionary.Add(key, value);
                return dictionary;
            }
            dictionary.Remove(key);
            dictionary.Add(key, value);

            return dictionary;
        }


        /// <summary>
        /// Checks if dictionary has the key and replaces existing value. 
        /// And if the key does not exist, adds new value
        /// </summary>
        /// <param name="dictionary">Dictionary to be modified</param>
        /// <param name="key">Key for the pair</param>
        /// <param name="value">Value to be added</param>
        /// <returns>Modified dictionary</returns>
        public static IDictionary<String, Object> AddOrReplace(this IDictionary<String, Object> dictionary, String key, String value)
        {
            object result;
            if (!dictionary.TryGetValue(key, out result))
            {
                dictionary.Add(key, value);
                return dictionary;
            }
            dictionary.Remove(key);
            dictionary.Add(key, value);

            return dictionary;
        }


        /// <summary>
        /// Checks if the key exists in the dictionary and adds new value.
        /// If key is already in the dictionary, nothing happens.
        /// </summary>
        /// <param name="data">Dictionary to be modified</param>
        /// <param name="key">Key for the pair</param>
        /// <param name="value">Valut to be added</param>
        /// <returns>Modified dictionary</returns>
        public static IDictionary<string, object> AddIfNotExist(this IDictionary<string, object> data, string key, string value)
        {
            if (!data.ContainsKey(key))
            {
                data.Add(key, value);
            }
            return data;
        }
    }
}
