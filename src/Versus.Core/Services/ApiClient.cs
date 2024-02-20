using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Versus.Core.Exceptions;
using Versus.Shared;
using Versus.Shared.Auth;

namespace Versus.Core.Services;

public class ApiClient
{
    private readonly HttpClient _client;

    public ApiClient(HttpClient client)
    {
        _client = client;
    }

    public string? TokenType { get; set; }

    public string? AccessToken
    {
        get => _client.DefaultRequestHeaders.Authorization?.Parameter;
        set
        {
            _client.DefaultRequestHeaders.Authorization = value == null
                ? null
                : new AuthenticationHeaderValue(TokenType ?? "Bearer", value);
        }
    }

    public string? RefreshToken { get; set; }

    public async Task GetAsync(string uri, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(uri, HttpMethod.Get, null, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!response.IsSuccessStatusCode)
        {
            var error = await GetErrorDetails(response, cancellationToken);
            throw new ApiException(response.StatusCode, error);
        }
    }

    public async Task<TResult?> GetAsync<TResult>(string uri, CancellationToken cancellationToken = default)
    {
        var response = await SendAsync(uri, HttpMethod.Get, null, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!response.IsSuccessStatusCode)
        {
            var error = await GetErrorDetails(response, cancellationToken);
            throw new ApiException(response.StatusCode, error);
        }

        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken);
    }

    public async Task PostAsync<TRequest>(string uri, TRequest? value, CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;
        if (value != null)
        {
            string json = JsonSerializer.Serialize(value);
            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await SendAsync(uri, HttpMethod.Post, content, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!response.IsSuccessStatusCode)
        {
            var error = await GetErrorDetails(response, cancellationToken);
            throw new ApiException(response.StatusCode, error);
        }
    }

    public async Task<TResult?> PostAsync<TRequest, TResult>(string uri, TRequest? value,
        CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;
        if (value != null)
        {
            string json = JsonSerializer.Serialize(value);
            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await SendAsync(uri, HttpMethod.Post, content, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!response.IsSuccessStatusCode)
        {
            var error = await GetErrorDetails(response, cancellationToken);
            throw new ApiException(response.StatusCode, error);
        }

        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken);
    }

    public async Task PutAsync<TRequest>(string uri, TRequest? value, CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;
        if (value != null)
        {
            string json = JsonSerializer.Serialize(value);
            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await SendAsync(uri, HttpMethod.Put, content, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!response.IsSuccessStatusCode)
        {
            var error = await GetErrorDetails(response, cancellationToken);
            throw new ApiException(response.StatusCode, error);
        }
    }

    public async Task<TResult?> PutAsync<TRequest, TResult>(string uri, TRequest? value,
        CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;
        if (value != null)
        {
            string json = JsonSerializer.Serialize(value);
            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await SendAsync(uri, HttpMethod.Put, content, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!response.IsSuccessStatusCode)
        {
            var error = await GetErrorDetails(response, cancellationToken);
            throw new ApiException(response.StatusCode, error);
        }

        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken);
    }

    public async Task DeleteAsync<TRequest>(string uri, TRequest? value, CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;
        if (value != null)
        {
            string json = JsonSerializer.Serialize(value);
            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await SendAsync(uri, HttpMethod.Delete, content, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        if (!response.IsSuccessStatusCode)
        {
            var error = await GetErrorDetails(response, cancellationToken);
            throw new ApiException(response.StatusCode, error);
        }
    }

    public async Task<TResult?> DeleteAsync<TRequest, TResult>(string uri, TRequest? value,
        CancellationToken cancellationToken = default)
    {
        HttpContent? content = null;
        if (value != null)
        {
            string json = JsonSerializer.Serialize(value);
            content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        var response = await SendAsync(uri, HttpMethod.Delete, content, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();

        if (!response.IsSuccessStatusCode)
        {
            var error = await GetErrorDetails(response, cancellationToken);
            throw new ApiException(response.StatusCode, error);
        }

        return await response.Content.ReadFromJsonAsync<TResult>(cancellationToken);
    }

    private async Task<HttpResponseMessage> SendAsync(string endpoint, HttpMethod method, HttpContent? content,
        CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(method, endpoint) { Content = content };
        var response = await _client.SendAsync(request, cancellationToken);
        if (response.StatusCode == HttpStatusCode.Unauthorized && RefreshToken != null)
        {
            var result = await RefreshTokenAsync(RefreshToken, cancellationToken);
            AccessToken = result.Item1;
            RefreshToken = result.Item2;

            response = await _client.SendAsync(request, cancellationToken);
        }

        return response;
    }

    private async Task<(string, string)> RefreshTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        const string endpoint = "/api/Auth/refresh-token";
        var request = new RefreshTokenRequest { Token = token };
        var response = await _client.PostAsJsonAsync(endpoint, request, cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>(cancellationToken);
        return (content!.AccessToken, content.RefreshToken);
    }

    private static async Task<ApiError?> GetErrorDetails(HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        try
        {
            string content = await response.Content.ReadAsStringAsync(cancellationToken);
            if (!string.IsNullOrEmpty(content))
            {
                return JsonSerializer.Deserialize<ApiError>(content);
            }
        }
        catch
        {
            // ignored
        }

        return default;
    }
}
