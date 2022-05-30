using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;
#endif

namespace Jellyfin.Plugin.JavTube.ExternalIds;

public class MovieExternalId : IExternalId
{
#if __EMBY__
    public string Name => Plugin.Instance.Name;
#else
    public string ProviderName => Plugin.Instance.Name;

    public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif
    public string Key => Plugin.Instance.Name;

    public string UrlFormatString => Plugin.Instance.Configuration.Server + "/#{0}";

    public bool Supports(IHasProviderIds item)
    {
        return item is Movie;
    }
}