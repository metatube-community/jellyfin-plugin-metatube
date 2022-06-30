using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class ProviderIdExtensions
{
    public static ProviderId GetPid(this IHasProviderIds instance, string name)
    {
        return !instance.ProviderIds.Any()
            ? new ProviderId()
            : ProviderId.Deserialize(instance.GetProviderId(name));
    }

    public static void SetPid(this IHasProviderIds instance, string name, string provider, string id,
        double? position = null, bool? updateInfo = null)
    {
        var pid = new ProviderId
        {
            Provider = provider,
            Id = id,
            Position = position,
            UpdateInfo = updateInfo
        };
        instance.SetProviderId(name, pid.Serialize());
    }
}