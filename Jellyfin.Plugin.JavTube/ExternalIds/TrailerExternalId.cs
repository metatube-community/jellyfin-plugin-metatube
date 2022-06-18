using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.JavTube.ExternalIds;

public class TrailerExternalId : BaseExternalId
{
#if __EMBY__
    public override string Name => Constant.JavTrailerId;
#else
    public override string ProviderName => Constant.JavTrailerId;

    public override ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

    public override string Key => Constant.JavTrailerId;

    public override string UrlFormatString => null;

    public override bool Supports(IHasProviderIds item)
    {
        return item is Movie;
    }
}