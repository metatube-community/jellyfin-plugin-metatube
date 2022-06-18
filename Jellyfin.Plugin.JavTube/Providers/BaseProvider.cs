using Jellyfin.Plugin.JavTube.Configuration;
using Jellyfin.Plugin.JavTube.Helpers;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;
using HttpRequestOptions = MediaBrowser.Common.Net.HttpRequestOptions;

#else
using Microsoft.Extensions.Logging;
using Jellyfin.Plugin.JavTube.Extensions;
#endif

namespace Jellyfin.Plugin.JavTube.Providers;

public abstract class BaseProvider
{
    protected readonly ILogger Logger;
    protected static PluginConfiguration Configuration => Plugin.Instance.Configuration;

#if __EMBY__
    private readonly IHttpClient _httpClient;

    protected BaseProvider(IHttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
#else
    private readonly IHttpClientFactory _httpClientFactory;

    protected BaseProvider(IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _httpClientFactory = httpClientFactory;
#endif
        Logger = logger;
    }

    public virtual int Order => 1;

    public virtual string Name => Plugin.Instance.Name;

#if __EMBY__
    public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
#else
    public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
#endif
    {
        Logger.Debug("GetImageResponse for url: {0}", url);
        return GetAsync(url, cancellationToken);
    }

#if __EMBY__
    private async Task<HttpResponseInfo> GetAsync(string url, CancellationToken cancellationToken)
    {
        return await _httpClient.GetResponse(new HttpRequestOptions
        {
            Url = url,
            CancellationToken = cancellationToken,
            UserAgent = HttpClientHelper.UserAgent
        }).ConfigureAwait(false);
    }
#else
    private async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(Name);
        httpClient.DefaultRequestHeaders
            .Add("User-Agent", HttpClientHelper.UserAgent);
        return await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
    }
#endif
}