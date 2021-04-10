namespace DW.ELA.Utility.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class DictionaryExtensions
    {
        public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
            where TValue : class
        {
            if (value == null)
                return;

            if (dictionary.ContainsKey(key))
                throw new ArgumentException("Key already present");

            dictionary.Add(key, value);
        }

        public static void AddIfNotNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue? value)
            where TValue : struct
        {
            if (value == null)
                return;

            if (dictionary.ContainsKey(key))
                throw new ArgumentException("Key already present");

            dictionary.Add(key, value.Value);
        }
    }
}
