namespace bilhete24.Exceptions;

public class PhoneAlreadyExistsException : Exception
{
    public PhoneAlreadyExistsException(string message) : base(message) { }
}