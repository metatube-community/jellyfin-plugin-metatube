using Jellyfin.Plugin.JavTube.Helpers;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;
#endif

namespace Jellyfin.Plugin.JavTube.ExternalIds;

public class TrailerExternalId : BaseExternalId
{
#if __EMBY__
    public override string Name => TrailerHelper.Name;
#else
    public override string ProviderName => TrailerHelper.Name;

    public override ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

    public override string Key => TrailerHelper.Name;

    public override string UrlFormatString => null;

    public override bool Supports(IHasProviderIds item)
    {
        return item is Movie;
    }
}