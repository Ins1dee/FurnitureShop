namespace Application.Extensions;

public static class DictionaryExtensions
{
    public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        List<TKey> keys,
        List<TValue> values) 
        where TKey : notnull
    {
        for (var i = 0; i < keys.Count; i++)
        {
            dictionary.Add(keys[i], values[i]);
        }

        return dictionary;
    }
}