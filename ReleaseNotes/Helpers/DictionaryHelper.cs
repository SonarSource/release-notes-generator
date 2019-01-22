using System.Collections.Generic;

namespace ReleaseNotes.Helpers
{
    public static class DictionaryHelper
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue)) =>
            dictionary.TryGetValue(key, out var value)
                ? value
                : defaultValue;
    }
}
