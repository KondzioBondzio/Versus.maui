using Versus.Core.Services.Session;

namespace Versus.Mobile.Services.Session;

public class SecureStorageSession : ISession
{
    private const string Prefix = "_session";
    private const string KeysKey = "__keys";
    private readonly HashSet<string> _keys;

    public SecureStorageSession()
    {
        _keys = SecureStorageWrapper.Get<HashSet<string>>(GetPrefixedKey(KeysKey)) ?? [];
    }

    public string Id { get; } = Guid.NewGuid().ToString();

    public IReadOnlyCollection<string> Keys => _keys;

    public T? Get<T>(string key)
    {
        string prefixedKey = GetPrefixedKey(key);
        return SecureStorageWrapper.Get<T>(prefixedKey);
    }

    public void Set<T>(string key, T value)
    {
        string prefixedKey = GetPrefixedKey(key);
        SecureStorageWrapper.Set(prefixedKey, value);
        _keys.Add(prefixedKey);
        SaveKeys();
    }

    public bool Remove(string key)
    {
        string prefixedKey = GetPrefixedKey(key);
        if (!_keys.Contains(prefixedKey))
        {
            return false;
        }

        SecureStorage.Remove(prefixedKey);
        _keys.Remove(prefixedKey);
        SaveKeys();

        return true;
    }

    public void Clear()
    {
        foreach (string key in _keys)
        {
            SecureStorage.Remove(key);
        }

        _keys.Clear();
        SaveKeys();
    }

    private static string GetPrefixedKey(string key) => $"{Prefix}_{key}";

    private void SaveKeys()
    {
        SecureStorageWrapper.Set(GetPrefixedKey(KeysKey), _keys);
    }
}
