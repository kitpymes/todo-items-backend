namespace TodoItems.Domain._Common.AppResults;

public interface IAppResultSuccess : IAppResultBase
{
    object? Data { get; }
}