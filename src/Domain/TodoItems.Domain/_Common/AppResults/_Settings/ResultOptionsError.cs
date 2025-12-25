using System.Net;

namespace TodoItems.Domain._Common.AppResults._Settings;

public class ResultOptionsError
{
    public ResultSettingsError ResultSettings { get; private set; } = new ResultSettingsError();

    public ResultOptionsError WithTitle(string title)
    {
        ResultSettings.Title = title;

        return this;
    }

    public ResultOptionsError WithStatusCode(HttpStatusCode status)
    {
        ResultSettings.Status = (int)status;

        return this;
    }

    public ResultOptionsError WithDetails(object details)
    {
        ResultSettings.Details = details;

        return this;
    }

    public ResultOptionsError WithExceptionType(string exception)
    {
        ResultSettings.Exception = exception;

        return this;
    }

    public ResultOptionsError WithMessage(string message)
    {
        ResultSettings.AddMessage(message);

        return this;
    }

    public ResultOptionsError WithMessages(IEnumerable<string> messages)
    {
        ResultSettings.AddMessages(messages);

        return this;
    }

    public ResultOptionsError WithErrors(IDictionary<string, IEnumerable<string>>? errors)
    {
        ResultSettings.AddErrors(errors);

        return this;
    }

    public ResultOptionsError WithErrors(IEnumerable<(string fieldName, string message)> errors)
    {
        if (errors is not null)
        {
            IDictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();

            foreach (var (fieldName, message) in errors)
            {
                if (!keyValuePairs.TryGetValue(fieldName, out List<string>? value))
                {
                    value = [];
                    keyValuePairs[fieldName] = value;
                }

                value.Add(message);
            }

            var result = keyValuePairs.ToDictionary(
                kvp => kvp.Key,
                kvp => (IEnumerable<string>)kvp.Value
            );

            ResultSettings.AddErrors(result);
        }

        return this;
    }
}
