using System.Web;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class BaseItemExtensions
{
    public static string GetTrailerUrl(this BaseItem item)
    {
        return !item.ProviderIds.Any()
            ? string.Empty
            : HttpUtility.UrlDecode(item.GetProviderId("TrailerUrl"));
    }

    public static void SetTrailerUrl(this BaseItem item, string url)
    {
        item.SetProviderId("TrailerUrl", HttpUtility.UrlEncode(url));
    }
}