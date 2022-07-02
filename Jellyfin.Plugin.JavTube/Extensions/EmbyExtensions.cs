#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;

namespace Jellyfin.Plugin.JavTube.Extensions;

public static class EmbyExtensions
{
    #region LogManager

    public static ILogger CreateLogger<T>(this ILogManager logManager)
    {
        return logManager.GetLogger($"{Plugin.Instance.Name}.{typeof(T).Name}");
    }

    #endregion

    #region HttpResponseMessage

    public static async Task<HttpResponseInfo> ToHttpResponseInfo(this HttpResponseMessage response)
    {
        if (response == null)
            throw new ArgumentNullException(nameof(response));

        return new HttpResponseInfo
        {
            Content = await response.Content.ReadAsStreamAsync(),
            ContentLength = response.Content.Headers.ContentLength,
            ContentType = response.Content.Headers.ContentType?.ToString(),
            StatusCode = response.StatusCode,
            Headers = response.Content.Headers.ToDictionary(kvp => kvp.Key, kvp => string.Join(", ", kvp.Value))
        };
    }

    #endregion
}

#endif