namespace GroupProject.Application.Common.Exceptions;

public class BadRequestException : ApplicationException
{
    public BadRequestException(string? message, Exception? innerException = null) : base(message, innerException)
    {
    }

    public BadRequestException(string? message, string? howToFix, string? howToPrevent) : base(message)
    {
        HowToFix = howToFix;
        HowToPrevent = howToPrevent;
    }
}
