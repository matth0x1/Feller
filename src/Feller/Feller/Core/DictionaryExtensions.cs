using System.Collections.Generic;

namespace Feller.Core
{
    internal static class DictionaryExtensions
    {
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue>? source)
        {
            if (source == null)
            {
                return;
            }

            foreach (var field in source)
            {
                target.AddOrUpdate(field.Key, field.Value);
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> target, TKey key, TValue value)
        {
            if (value == null)
            {
                return;
            }

            target[key] = value;
        }
    }
}
