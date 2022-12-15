using Jellyfin.Plugin.MetaTube.Configuration;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;

#else
using Microsoft.Extensions.Logging;
using Jellyfin.Plugin.MetaTube.Extensions;
#endif

namespace Jellyfin.Plugin.MetaTube.Providers;

public abstract class BaseProvider
{
    protected readonly ILogger Logger;

    protected BaseProvider(ILogger logger)
    {
        Logger = logger;
    }

    protected static PluginConfiguration Configuration => Plugin.Instance.Configuration;

    public virtual int Order => 1;

    public virtual string Name => Plugin.Instance.Name;

#if __EMBY__
    public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
#else
    public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
#endif
    {
        Logger.Debug("GetImageResponse for url: {0}", url);
        return ApiClient.GetImageResponse(url, cancellationToken);
    }
}