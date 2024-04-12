using IHttpSession = Microsoft.AspNetCore.Http.ISession;
using ISession = Versus.Core.Services.Session.ISession;

namespace Versus.Web.Services.Session;

public class HttpStorageSession : ISession
{
    private const string Prefix = "_session";
    private readonly IHttpContextAccessor _httpContext;

    public HttpStorageSession(IHttpContextAccessor httpContext)
    {
        _httpContext = httpContext;
    }

    private IHttpSession Session => _httpContext.HttpContext!.Session;

    public string Id => Session.Id;

    public IReadOnlyCollection<string> Keys => Session.Keys.ToHashSet();

    public T? Get<T>(string key)
    {
        string prefixedKey = GetPrefixedKey(key);
        return Session.Get<T>(prefixedKey);
    }

    public void Set<T>(string key, T value)
    {
        string prefixedKey = GetPrefixedKey(key);
        Session.Set(prefixedKey, value);
    }

    public bool Remove(string key)
    {
        string prefixedKey = GetPrefixedKey(key);
        Session.Remove(prefixedKey);
        return true;
    }

    public void Clear()
    {
        Session.Clear();
    }

    private static string GetPrefixedKey(string key)
    {
        return $"{Prefix}_{key}";
    }
}