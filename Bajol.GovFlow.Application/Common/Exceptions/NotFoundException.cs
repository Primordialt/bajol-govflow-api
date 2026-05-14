namespace Bajol.GovFlow.Application.Common.Exceptions;

public sealed class NotFoundException : AppException
{
    public NotFoundException(string message, string? code = null)
        : base(message, 404, code)
    {
    }
}
