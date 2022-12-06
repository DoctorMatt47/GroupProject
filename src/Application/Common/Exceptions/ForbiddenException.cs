namespace GroupProject.Application.Common.Exceptions;

public class ForbiddenException : ApplicationException
{
    public ForbiddenException(string? message, Exception? innerException = null) : base(message, innerException)
    {
        HowToFix = "Sign in to an account with the required permissions";
        HowToPrevent = "Do not try to get forbidden information";
    }
}
