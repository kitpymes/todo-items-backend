namespace TodoItems.Domain._Common.Exceptions;

public class DomainValidationException(string message) : Exception(message)
{
}