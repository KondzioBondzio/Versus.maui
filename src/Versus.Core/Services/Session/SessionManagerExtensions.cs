namespace Versus.Core.Services.Session;

public static class SessionManagerExtensions
{
    public static string? GetId(this SessionManager sessionManager)
    {
        return sessionManager.Session.Get<string>("Id");
    }

    public static void SetId(this SessionManager sessionManager, string id)
    {
        sessionManager.Session.Set<string>("Id", id);
    }
}
