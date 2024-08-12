using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;
#endif

namespace Jellyfin.Plugin.MetaTube.ExternalIds;

public class GfriendsExternalId : BaseExternalId
{
#if __EMBY__
    public override string Name => "Gfriends";
#else
    public override string ProviderName => "Gfriends";

    public override ExternalIdMediaType? Type => ExternalIdMediaType.Person;
#endif

    public override string Key => "Gfriends";

    public override string UrlFormatString => null;

    public override bool Supports(IHasProviderIds item)
    {
        return item is Person;
    }
}