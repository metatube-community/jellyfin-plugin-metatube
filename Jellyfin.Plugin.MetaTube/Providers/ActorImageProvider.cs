using Jellyfin.Plugin.MetaTube.Extensions;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
#if __EMBY__
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Configuration;

#else
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.MetaTube.Providers;

public class ActorImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
{
#if __EMBY__
    public ActorImageProvider(ILogManager logManager) : base(logManager.CreateLogger<ActorImageProvider>())
#else
    public ActorImageProvider( ILogger<ActorImageProvider> logger) : base(logger)
#endif
    {
    }

#if __EMBY__
    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, LibraryOptions libraryOptions,
        CancellationToken cancellationToken)
#else
    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
#endif
    {
        var pid = item.GetPid(Plugin.ProviderId);
        if (string.IsNullOrWhiteSpace(pid.Id) || string.IsNullOrWhiteSpace(pid.Provider))
            return Enumerable.Empty<RemoteImageInfo>();

        var actorInfo = await ApiClient.GetActorInfoAsync(pid.Provider, pid.Id, cancellationToken);

        if (actorInfo.Images?.Any() != true)
            return Enumerable.Empty<RemoteImageInfo>();

        return actorInfo.Images.Select(image => new RemoteImageInfo
        {
            ProviderName = Name,
            Type = ImageType.Primary,
            Url = ApiClient.GetPrimaryImageApiUrl(actorInfo.Provider, actorInfo.Id, image, 0.5, true)
        });
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