
namespace TodoItems.Domain._Common.AppResults;

public interface IAppResultError : IAppResultBase
{
    string? TraceId { get; }

    string? Exception { get; }

    string? Message { get; }

    public IDictionary<string, IEnumerable<string>>? Errors { get; }

    object? Details { get; }
}
