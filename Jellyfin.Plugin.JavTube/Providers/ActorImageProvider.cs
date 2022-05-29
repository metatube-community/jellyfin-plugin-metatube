using Jellyfin.Plugin.JavTube.Extensions;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Configuration;
#else
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.JavTube.Providers;

public class ActorImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
{
#if __EMBY__
    public ActorImageProvider(IHttpClient httpClient, ILogManager logManager) :
        base(httpClient, logManager.CreateLogger<ActorImageProvider>())
#else
        public ActorImageProvider(IHttpClientFactory httpClientFactory, ILogger<ActorImageProvider> logger) : base(
            httpClientFactory, logger)
#endif
    {
        // Nothing
    }

    public int Order => 1;

    public string Name => Constant.JavTube;

#if __EMBY__
    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, LibraryOptions libraryOptions,
        CancellationToken cancellationToken)
#else
        public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
#endif
    {
        var pm = item.GetProviderIdModel(Name);
        if (string.IsNullOrWhiteSpace(pm.Id) || string.IsNullOrWhiteSpace(pm.Provider))
            return new List<RemoteImageInfo>();

        var actorInfo = await ApiClient.GetActorInfo(pm.Id, pm.Provider, cancellationToken);

        return actorInfo.Images.Select(image => new RemoteImageInfo
        {
            ProviderName = Name, 
            Type = ImageType.Primary,
            Url = ApiClient.GetPrimaryImageApiUrl(actorInfo.Id, actorInfo.Provider, image)
        }).ToList();
    }

    public bool Supports(BaseItem item)
    {
        return item is Person;
    }

    public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
    {
        return new List<ImageType>
        {
            ImageType.Primary
        };
    }
}