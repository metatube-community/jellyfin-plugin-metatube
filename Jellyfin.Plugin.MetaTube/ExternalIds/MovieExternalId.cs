using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;
#endif

namespace Jellyfin.Plugin.MetaTube.ExternalIds;

public class MovieExternalId : BaseExternalId
{
#if !__EMBY__
    public override ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

    public override bool Supports(IHasProviderIds item)
    {
        return item is Movie;
    }
}