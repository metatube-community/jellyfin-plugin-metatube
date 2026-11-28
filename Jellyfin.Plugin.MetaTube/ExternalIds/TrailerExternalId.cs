using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;
#endif

namespace Jellyfin.Plugin.MetaTube.ExternalIds;

public class TrailerExternalId : BaseExternalId
{
#if __EMBY__
    public override string Name => "TrailerUrl";
#else
    public override string ProviderName => "TrailerUrl";

    public override ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

    public override string Key => "TrailerUrl";

    public override string UrlFormatString => null;

    public override bool Supports(IHasProviderIds item)
    {
        return item is Movie;
    }
}