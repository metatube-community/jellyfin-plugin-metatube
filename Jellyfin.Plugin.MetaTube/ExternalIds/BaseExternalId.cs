using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;
#endif

namespace Jellyfin.Plugin.MetaTube.ExternalIds;

public abstract class BaseExternalId : IExternalId
{
#if __EMBY__
    public virtual string Name => Plugin.ProviderName;
#else
    public virtual string ProviderName => Plugin.ProviderName;

    public abstract ExternalIdMediaType? Type { get; }
#endif

    public virtual string Key => Plugin.ProviderId;

    public virtual string UrlFormatString => Plugin.Instance.Configuration.Server + "?redirect={0}";

    public abstract bool Supports(IHasProviderIds item);
}