namespace bilhete24.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string message) : base(message) { }
}