using Microsoft.AspNetCore.Http;
using System.Net;
using System.Net.Mime;

namespace TodoItems.Infrastructure.Extensions;

public static class HttpResponseExtensions
{
    public static async Task ToResultAsync(
        this HttpResponse httpResponse,
        HttpStatusCode status,
        string message,
        string contentType = MediaTypeNames.Application.Json,
        IDictionary<string, IEnumerable<string>>? headers = null)
    {
        if (httpResponse is not null)
        {
            httpResponse.Clear();
            httpResponse.StatusCode = (int)status;
            httpResponse.ContentType = contentType;

            if (headers?.Count > 0)
            {
                foreach (var (key, values) in headers)
                {
                    httpResponse.Headers.AppendList(key, values.ToList());
                }
            }
        }

        await httpResponse.WriteAsync(message).ConfigureAwait(false);
    }
}
