using System.Web;
using Jellyfin.Plugin.JavTube.Common;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.JavTube.Extensions;

public static class ProviderIdsExtensions
{
    public static ProviderId GetPid(this IHasProviderIds instance, string name)
    {
        return ProviderId.Parse(instance.GetProviderId(name));
    }

    public static void SetPid(this IHasProviderIds instance, string name, string provider, string id,
        double? position = null, bool? update = null)
    {
        var pid = new ProviderId
        {
            Provider = provider,
            Id = id,
            Position = position,
            Update = update
        };
        instance.SetProviderId(name, pid.ToString());
    }

    public static string GetTrailerUrl(this IHasProviderIds instance)
    {
        return !instance.ProviderIds.Any()
            ? string.Empty
            : HttpUtility.UrlDecode(instance.GetProviderId("TrailerUrl"));
    }

    public static void SetTrailerUrl(this IHasProviderIds instance, string url)
    {
        instance.SetProviderId("TrailerUrl", HttpUtility.UrlEncode(url));
    }
}