using Versus.Shared.Auth;

namespace Versus.Core.Services.Session;

public class SessionManager
{
    private readonly ApiClient _client;

    public SessionManager(ISession session, ApiClient client)
    {
        Session = session;
        _client = client;
    }

    public ISession Session { get; }

    public async Task LoginAsync(LoginViewModel model, CancellationToken cancellationToken = default)
    {
        const string endpoint = "api/Auth/login";
        var request = new LoginRequest
        {
            Login = model.Login, Password = model.Password
        };
        var response = await _client.PostAsync<LoginRequest, LoginResponse>(endpoint, request, cancellationToken);
        _client.TokenType = response!.TokenType;
        _client.AccessToken = response.AccessToken;
        _client.RefreshToken = response.RefreshToken;
    }

    public async Task RegisterAsync(RegisterViewModel model, CancellationToken cancellationToken = default)
    {
        const string endpoint = "api/Auth/register";
        // var request = new RegisterRequest();
        // await _client.PostAsync(endpoint, request, cancellationToken);
    }

    public Task LogoutAsync(CancellationToken cancellationToken)
    {
        _client.AccessToken = null;
        _client.RefreshToken = null;
        Session.Clear();
        return Task.CompletedTask;
    }
}