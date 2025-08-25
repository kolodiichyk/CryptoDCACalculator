namespace CryptoDCACalculator.Helpers;

public static class DictionaryExtensions
{
    public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
    {
        value = default(T);

        if (dictionary.TryGetValue(key, out object objValue))
        {
            if (objValue is T castValue)
            {
                value = castValue;
                return true;
            }
        }

        return false;
    }

    public static T GetValueOrDefault<T>(this IDictionary<string, object> dictionary, string key, T defaultValue = default(T))
    {
        return dictionary.TryGetValue(key, out T value) ? value : defaultValue;
    }
}
