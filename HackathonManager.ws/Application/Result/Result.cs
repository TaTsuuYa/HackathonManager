namespace HackathonManager.ws.Application.Result;

public class Result<T>
{
    public bool Success { get; init; }
    public T? Value { get; init; }
    public string? Error { get; init; }
    public int StatusCode { get; init; }

    public static Result<T> Ok(T value, int statusCode = StatusCodes.Status200OK) => new()
    {
        Success = true,
        Value = value,
        StatusCode = statusCode
    };

    public static Result<T> Fail(string error, int statusCode) => new()
    {
        Success = false,
        StatusCode = statusCode,
        Error = error
    };
}
