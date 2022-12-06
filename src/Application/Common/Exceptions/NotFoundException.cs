namespace GroupProject.Application.Common.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string? message, Exception? innerException = null) : base(message, innerException)
    {
        HowToFix = "Try other one";
        HowToPrevent = "Do not enter id manually";
    }
}
