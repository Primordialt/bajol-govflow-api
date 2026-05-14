namespace Bajol.GovFlow.Application.Common.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message, string? code = null)
        : base(message, 409, code)
    {
    }
}
