using System.Text.Json;

namespace Versus.Mobile.Services.Session;

public static class SecureStorageWrapper
{
    public static T? Get<T>(string key)
    {
        return GetInternal<T?>(key, default);
    }

    public static T Get<T>(string key, T defaultValue)
    {
        return GetInternal(key, defaultValue);
    }

    public static void Set<T>(string key, T? value)
    {
        if (value == null)
        {
            SecureStorage.Remove(key);
            return;
        }

        SecureStorage.SetAsync(key, JsonSerializer.Serialize(value));
    }

    private static T GetInternal<T>(string key, T defaultValue)
    {
        string? json = SecureStorage.GetAsync(key).Result;
        if (string.IsNullOrWhiteSpace(json))
        {
            return defaultValue;
        }

        try
        {
            return (T?)JsonSerializer.Deserialize(json, typeof(T)) ?? defaultValue;
        }
        catch
        {
            return defaultValue;
        }
    }
}
