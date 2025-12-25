using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace TodoItems.Infrastructure.Extensions;

public static class JsonExtensions
{
    public static string ToSerialize<T>(this T value, Action<JsonSerializerOptions>? action = null)
       => JsonSerializer.Serialize(value, typeof(T), action.ToConfigureOrDefault(new JsonSerializerOptions
       {
           Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
           WriteIndented = true,
       }));

    public static T? ToDeserialize<T>(this string value, Action<JsonSerializerOptions>? action = null)
    => JsonSerializer.Deserialize<T>(value, action.ToConfigureOrDefault(new JsonSerializerOptions
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
    }));
}
