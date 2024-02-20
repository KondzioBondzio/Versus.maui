using Versus.Core.Exceptions;
using Versus.Core.Services;
using Versus.Shared.Auth;

var httpClient = new HttpClient
{
    BaseAddress = new Uri("http://localhost:8080")
};
var apiClient = new ApiClient(httpClient);

await Register(apiClient);
await Login(apiClient);

static async Task Register(ApiClient apiClient)
{
    const string endpoint = "api/Auth/register";
    var request = new RegisterRequest
    {
        Login = "",
        Password = "",
        Email = ""
    };

    try
    {
        await apiClient.PostAsync(endpoint, request);
    }
    catch (ApiException ex)
    {
        Console.WriteLine(ex.Error?.Detail);
    }
}

static async Task Login(ApiClient apiClient)
{
    const string endpoint = "api/Auth/login";
    var request = new LoginRequest
    {
        Login = "",
        Password = ""
    };

    try
    {
        var response = await apiClient.PostAsync<LoginRequest, LoginResponse>(endpoint, request);
        apiClient.TokenType = response!.TokenType;
        apiClient.AccessToken = response.AccessToken;
        apiClient.RefreshToken = response.RefreshToken;

        Console.WriteLine($"Token Type:     {response.TokenType}");
        Console.WriteLine($"Access Token:   {response.AccessToken}");
        Console.WriteLine($"Refresh Token:  {response.RefreshToken}");
    }
    catch (ApiException ex)
    {
        Console.WriteLine(ex.Error?.Detail);
    }
}
