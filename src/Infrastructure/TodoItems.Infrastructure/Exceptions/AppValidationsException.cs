namespace TodoItems.Infrastructure.Exceptions;

[Serializable]
public class AppValidationsException : Exception
{
    public AppValidationsException(params string[] messages)
        : this(string.Join(", ", messages)) { }

    public AppValidationsException(IDictionary<string, IEnumerable<string>> errors)
    => Errors = errors;

    protected AppValidationsException(string message, Exception innerException)
        : base(message, innerException) { }

    private AppValidationsException() { }

    private AppValidationsException(string message)
        : base(message) { }

    public IDictionary<string, IEnumerable<string>>? Errors { get; }

    public bool HasErrors => Errors?.Count > 0 || !string.IsNullOrWhiteSpace(Message);

    public bool Contains(string message)
    => Message != null && Message.Contains(message, StringComparison.CurrentCulture);

    public bool Contains(string fieldName, string message)
    => Errors != null && Errors[fieldName] != null && Errors[fieldName].ToList().Contains(message);
}