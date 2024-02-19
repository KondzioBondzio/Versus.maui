namespace Versus.Core.Services.Session;

public interface ISession
{
    string Id { get; }
    IReadOnlyCollection<string> Keys { get; }

    T? Get<T>(string key);
    void Set<T>(string key, T value);
    bool Remove(string key);
    void Clear();
}
