using Jellyfin.Plugin.JavTube.Models;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class ProviderIdsExtension
{
    private const char Separator = '#';

    public static ProviderIdModel GetProviderIdModel(this IHasProviderIds instance, string name)
    {
        if (!instance.ProviderIds.Any())
            return new ProviderIdModel();

        var providerIds = instance.GetProviderId(name)?.Split(Separator);
        return new ProviderIdModel
        {
            Provider = providerIds?.Length > 1 ? providerIds[0] : string.Empty,
            Id = providerIds?.Length > 1 ? providerIds[1] : string.Empty
        };
    }

    public static void SetProviderIdModel(this IHasProviderIds instance, string name, ProviderIdModel pid)
    {
        instance.SetProviderId(name, string.Join(Separator, pid.Provider, pid.Id));
    }
}