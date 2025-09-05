using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace DotnetTestTask.Api;

public static class HttpContextExtensions
{
    /// <summary>
    /// Returns all params (query + body) as JSON.
    /// </summary>
    public static async Task<string> GetRequestParametersAsJsonAsync(this HttpContext context)
    {
        var queryParams = context.Request.Query
            .ToDictionary(k => k.Key, v => v.Value.ToString());

        string bodyParams = "{}";

        if (context.Request.ContentLength > 0 && context.Request.Body.CanSeek)
        {
            context.Request.Body.Position = 0;
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            bodyParams = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
        }

        var allParams = new
        {
            Query = queryParams,
            Body = bodyParams
        };

        return JsonSerializer.Serialize(allParams);
    }
}
