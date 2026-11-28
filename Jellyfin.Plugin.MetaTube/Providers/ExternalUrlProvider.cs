#if !__EMBY__
using Jellyfin.Plugin.MetaTube.ExternalIds;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.MetaTube.Providers;

public class ExternalUrlProvider : IExternalUrlProvider
{
    public string Name => Plugin.ProviderName;

    public IEnumerable<string> GetExternalUrls(BaseItem item)
    {
        if (item is Movie or Person && item.TryGetProviderId(Plugin.ProviderId, out var pid))
            yield return string.Format(BaseExternalId.RedirectUrlFormat, pid);
    }
}
#endif