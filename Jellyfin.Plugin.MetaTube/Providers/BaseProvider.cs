using Jellyfin.Plugin.MetaTube.Configuration;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;
using MediaBrowser.Controller.Providers;

#else
using Microsoft.Extensions.Logging;
using Jellyfin.Plugin.MetaTube.Extensions;
#endif

namespace Jellyfin.Plugin.MetaTube.Providers;

#if __EMBY__
public abstract class BaseProvider : IHasSupportedExternalIdentifiers
#else
public abstract class BaseProvider
#endif
{
    protected readonly ILogger Logger;

    protected BaseProvider(ILogger logger)
    {
        Logger = logger;
    }

    protected static PluginConfiguration Configuration => Plugin.Instance.Configuration;

    public virtual int Order => 1;

    public virtual string Name => Plugin.ProviderName;

#if __EMBY__
    public string[] GetSupportedExternalIdentifiers()
    {
        return new[] { Plugin.ProviderName };
    }
#endif

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