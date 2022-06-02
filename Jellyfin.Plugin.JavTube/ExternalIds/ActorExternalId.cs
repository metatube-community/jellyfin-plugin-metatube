using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;
#endif

namespace Jellyfin.Plugin.JavTube.ExternalIds;

public class ActorExternalId : BaseExternalId, IExternalId
{
#if !__EMBY__
    public override ExternalIdMediaType? Type => ExternalIdMediaType.Person;
#endif

    public override bool Supports(IHasProviderIds item)
    {
        return item is Person;
    }
}