using System.Net;
using System.Text.Json.Serialization;
using TodoItems.Domain._Common.AppResults._Settings;

namespace TodoItems.Domain._Common.AppResults;

public class AppResult : IAppResult
{
    [JsonConstructor]
    private AppResult(bool isSuccess) => IsSuccess = isSuccess;

    public bool IsSuccess { get; private set; }

    public int? Status { get; private set; }

    public string? Title { get; private set; }

    public string? TraceId { get; private set; }

    public string? Exception { get; private set; }

    public string? Message { get; private set; }

    public object? Details { get; private set; }

    public object? Data { get; private set; }

    public IDictionary<string, IEnumerable<string>>? Errors { get; private set; }

    public static IAppResult Success() => new AppResult(true);

    public static IAppResult Success<TValue>(TValue value) => new AppResult(true)
    {
        Data = value
    };

    public static IAppResult BadRequest(string message)
    => Error(x => x
        .WithMessage(message)
        .WithStatusCode(HttpStatusCode.BadRequest)
        .WithTitle("Validation Error"));

    public static IAppResult BadRequest(IEnumerable<string> messages)
        => Error(x => x
            .WithMessages(messages)
            .WithStatusCode(HttpStatusCode.BadRequest)
            .WithTitle("Validation Error"));

    public static IAppResult BadRequest(IEnumerable<(string fieldName, string message)> errors)
        => Error(x => x
            .WithErrors(errors)
            .WithStatusCode(HttpStatusCode.BadRequest)
            .WithTitle("Validation Error"));

    public static IAppResult Error(Action<ResultOptionsError> options)
    {
        var settings = new ResultOptionsError();

        options.Invoke(settings);

        var config = settings.ResultSettings;

        return new AppResult(false)
        {
            Status = config.Status,
            Title = config.Title,
            TraceId = Guid.NewGuid().ToString(),
            Details = config.Details,
            Exception = config.Exception,
            Message = config.Messages?.Any() == true ? string.Join(", ", config.Messages) : null,
            Errors = config.Errors,
        };
    }
}