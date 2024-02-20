using System.Net;
using Versus.Shared;

namespace Versus.Core.Exceptions;

public class ApiException(HttpStatusCode statusCode) : Exception
{
    public ApiException(HttpStatusCode statusCode, ApiError? error)
        : this(statusCode)
    {
        Error = error;
    }

    public HttpStatusCode StatusCode { get; } = statusCode;
    public ApiError? Error { get; }
}
