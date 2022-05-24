using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
#if !__EMBY__
using MediaBrowser.Model.Providers;
#endif

namespace Jellyfin.Plugin.JavTube.ExternalIds;

public class JavTubeExternalId: IExternalId
{
#if __EMBY__
    public string Name => Constant.JavTube;
#else
        public string ProviderName => Constant.JavTube;
#endif
    public string Key => Constant.JavTube;

    public string UrlFormatString => "{0}";

#if !__EMBY__
        public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

    public bool Supports(IHasProviderIds item)
    {
        return item is Movie;
    }
}

public class JavTubeIdExternalId: IExternalId
{
#if __EMBY__
    public string Name => Constant.JavTubeId;
#else
    public string ProviderName => Constant.JavTubeId;
#endif
    public string Key => Constant.JavTubeId;

    public string UrlFormatString => null;

#if !__EMBY__
    public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

    public bool Supports(IHasProviderIds item)
    {
        return item is Movie;
    }
}

public class JavTubeProviderExternalId: IExternalId
{
#if __EMBY__
    public string Name => Constant.JavTubeProvider;
#else
    public string ProviderName => Constant.JavTubeProvider;
#endif
    public string Key => Constant.JavTubeProvider;

    public string UrlFormatString => null;

#if !__EMBY__
    public ExternalIdMediaType? Type => ExternalIdMediaType.Movie;
#endif

    public bool Supports(IHasProviderIds item)
    {
        return item is Movie;
    }
}
