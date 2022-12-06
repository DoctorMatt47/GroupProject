namespace GroupProject.Application.Common.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException(string? message, Exception? innerException = null) : base(message, innerException)
    {
    }

    public string? HowToFix { get; protected set; }
    public string? HowToPrevent { get; protected set; }
}
