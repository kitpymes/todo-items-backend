using System.Diagnostics.CodeAnalysis;

namespace TodoItems.Infrastructure.Extensions;

public static class ActionExtensions
{
    [return: NotNull]
    public static TOptions ToConfigureOrDefault<TOptions>(this Action<TOptions>? action, TOptions? defaultOptions = null)
        where TOptions : class, new()
    {
        defaultOptions ??= new TOptions();

        action?.Invoke(defaultOptions);

        return defaultOptions;
    }
}
