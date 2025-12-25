namespace TodoItems.Domain._Common.AppResults._Settings;

public class ResultSettingsError
{
    public int? Status { get; set; }

    public string? Title { get; set; }

    public string? TraceId { get; set; }

    public string? Exception { get; set; }

    public IList<string>? Messages { get; private set; }

    public IDictionary<string, IEnumerable<string>>? Errors { get; private set; }

    public object? Details { get; set; }

    public void AddMessage(string message)
    {
        Messages ??= [];

        if (!Messages.Contains(message))
        {
            Messages.Add(message);
        }
    }

    public void AddMessages(IEnumerable<string> messages)
    {
        foreach (var message in messages)
        {
            AddMessage(message);
        }
    }

    public void AddErrors(IDictionary<string, IEnumerable<string>>? errors)
    {
        Errors ??= new Dictionary<string, IEnumerable<string>>();

        Errors = errors;
    }
}
