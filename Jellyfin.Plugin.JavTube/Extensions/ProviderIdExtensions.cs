using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class ProviderIdExtensions
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
}