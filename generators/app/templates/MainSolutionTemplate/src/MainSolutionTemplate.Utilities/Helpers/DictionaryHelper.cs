using System;
using System.Collections.Generic;

namespace MainSolutionTemplate.Utilities.Helpers
{
    public static class DictionaryHelper
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> getValue)
        {
            lock (dictionary)
            {
                if (dictionary.ContainsKey(key))
                {
                    return dictionary[key];
                }
                var value = getValue(key);
                dictionary.Add(key,value);
                return value;
            }
        }
    }
}