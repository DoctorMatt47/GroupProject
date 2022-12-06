namespace GroupProject.Application.Common.Exceptions;

public class ConflictException : ApplicationException
{
    public ConflictException(string? message, Exception? innerException = null) : base(message, innerException)
    {
        HowToFix = "Try other one";
        HowToPrevent = "Do not try to add existing resource";
    }
}
