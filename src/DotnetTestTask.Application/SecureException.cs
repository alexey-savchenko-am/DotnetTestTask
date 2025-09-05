namespace DotnetTestTask.Core.Exceptions;

public class SecureException : Exception
{
    public SecureException(string message)
        : base(message)
    {
    }
}
