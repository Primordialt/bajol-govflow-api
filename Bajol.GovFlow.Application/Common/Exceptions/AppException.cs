namespace Bajol.GovFlow.Application.Common.Exceptions;

public class AppException : Exception
{
    public AppException(string message, int statusCode = 400, string? code = null)
        : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public int StatusCode { get; }
    public string? Code { get; }
}
