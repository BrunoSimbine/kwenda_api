namespace bilhete24.Exceptions;

public class UserOrPassInvalidException : Exception
{
    public UserOrPassInvalidException(string message) : base(message) { }
}