#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;
using HttpRequestOptions = MediaBrowser.Common.Net.HttpRequestOptions;

#else
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.JavTube.Providers;

public abstract class BaseProvider
{
    private readonly ILogger _logger;

#if __EMBY__
    private readonly IHttpClient _httpClient;
    protected BaseProvider(IHttpClient httpClient, ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }
#else
    private readonly IHttpClientFactory _httpClientFactory;
    protected BaseProvider(IHttpClientFactory httpClientFactory, ILogger logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }
#endif
    
    public int Order => 1;

    public string Name => Plugin.Instance.Name;

#if __EMBY__
    public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
#else
    public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
#endif
    {
        LogInfo("GetImageResponse for url: {0}", url);
        return GetAsync(url, cancellationToken);
    }

#if __EMBY__
    private async Task<HttpResponseInfo> GetAsync(string url, CancellationToken cancellationToken)
    {
        return await _httpClient.GetResponse(new HttpRequestOptions
        {
            Url = url,
            EnableDefaultUserAgent = false,
            UserAgent = ApiClient.UserAgent,
            CancellationToken = cancellationToken
        }).ConfigureAwait(false);
    }
#else
    private async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", ApiClient.UserAgent);
        return await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
    }
#endif

    protected void LogInfo(string message, params object[] args)
    {
#if __EMBY__
        _logger.Info(message, args);
#else
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.LogInformation(message, args);
#endif
    }

    protected void LogError(string message, params object[] args)
    {
#if __EMBY__
        _logger.Error(message, args);
#else
        // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
        _logger.LogError(message, args);
#endif
    }
}