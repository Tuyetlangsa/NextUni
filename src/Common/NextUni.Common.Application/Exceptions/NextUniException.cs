using NextUni.Common.Domain;

namespace NextUni.Common.Application.Exceptions;

public sealed class NextUniException : Exception
{
    public NextUniException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}