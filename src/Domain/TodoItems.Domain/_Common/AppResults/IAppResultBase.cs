namespace TodoItems.Domain._Common.AppResults;

public interface IAppResultBase
{
    bool IsSuccess { get; }

    int? Status { get; }

    string? Title { get; }
}
