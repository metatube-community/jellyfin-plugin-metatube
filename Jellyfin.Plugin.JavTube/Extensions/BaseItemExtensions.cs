using System.Web;
using Jellyfin.Plugin.JavTube.Helpers;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class BaseItemExtensions
{
    public static string GetTrailerUrl(this BaseItem item)
    {
        return !item.ProviderIds.Any()
            ? string.Empty
            : HttpUtility.UrlDecode(item.GetProviderId(TrailerHelper.Name));
    }

    public static void SetTrailerUrl(this BaseItem item, string url)
    {
        item.SetProviderId(TrailerHelper.Name, HttpUtility.UrlEncode(url));
    }
}