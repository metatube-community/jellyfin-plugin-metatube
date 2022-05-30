using Jellyfin.Plugin.JavTube.Extensions;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Configuration;
using MediaBrowser.Model.Logging;

#else
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.JavTube.Providers;

public class ImageProvider : BaseProvider, IRemoteImageProvider, IHasOrder
{
#if __EMBY__
    public ImageProvider(IHttpClient httpClient, ILogManager logManager) : base(
        httpClient,
        logManager.CreateLogger<ImageProvider>())
#else
    public ImageProvider(IHttpClientFactory httpClientFactory, ILogger<ImageProvider> logger) : base(
        httpClientFactory, logger)
#endif
    {
        // Init
    }

#if __EMBY__
    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, LibraryOptions libraryOptions,
        CancellationToken cancellationToken)
#else
    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
#endif
    {
        var pid = item.GetProviderIdModel(Name);
        if (string.IsNullOrWhiteSpace(pid.Id) || string.IsNullOrWhiteSpace(pid.Provider))
            return new List<RemoteImageInfo>();

        var m = await ApiClient.GetMovieInfo(pid.Id, pid.Provider, cancellationToken);
        var images = new List<RemoteImageInfo>
        {
            new()
            {
                ProviderName = Name,
                Type = ImageType.Primary,
                Url = ApiClient.GetPrimaryImageApiUrl(m.Id, m.Provider)
            },
            new()
            {
                ProviderName = Name,
                Type = ImageType.Thumb,
                Url = ApiClient.GetThumbImageApiUrl(m.Id, m.Provider)
            },
            new()
            {
                ProviderName = Name,
                Type = ImageType.Backdrop,
                Url = ApiClient.GetBackdropImageApiUrl(m.Id, m.Provider)
            }
        };

        foreach (var imageUrl in m.PreviewImages)
        {
            images.Add(new RemoteImageInfo
            {
                ProviderName = Name,
                Type = ImageType.Primary,
                Url = ApiClient.GetPrimaryImageApiUrl(m.Id, m.Provider, imageUrl)
            });

            images.Add(new RemoteImageInfo
            {
                ProviderName = Name,
                Type = ImageType.Thumb,
                Url = ApiClient.GetThumbImageApiUrl(m.Id, m.Provider, imageUrl)
            });

            images.Add(new RemoteImageInfo
            {
                ProviderName = Name,
                Type = ImageType.Backdrop,
                Url = ApiClient.GetBackdropImageApiUrl(m.Id, m.Provider, imageUrl)
            });
        }

        return images;
    }

    public bool Supports(BaseItem item)
    {
        return item is Movie;
    }

    public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
    {
        return new List<ImageType>
        {
            ImageType.Primary,
            ImageType.Thumb,
            ImageType.Backdrop
        };
    }
}