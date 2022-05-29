using Jellyfin.Plugin.JavTube.Models;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class ProviderIdsExtension
{
    private const char Separator = '#';

    public static ProviderInfoModel GetProviderModel(this IHasProviderIds instance, string name)
    {
        if (!instance.ProviderIds.Any())
            return new ProviderInfoModel();

        var providerIds = instance.GetProviderId(name)?.Split(Separator);
        return new ProviderInfoModel
        {
            Provider = providerIds?.Length > 1 ? providerIds[0] : string.Empty,
            Id = providerIds?.Length > 1 ? providerIds[1] : string.Empty
        };
    }

    public static void SetProviderModel(this IHasProviderIds instance, string name, ProviderInfoModel pm)
    {
        instance.SetProviderId(name, string.Join(Separator, pm.Provider, pm.Id));
    }
}