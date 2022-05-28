using Jellyfin.Plugin.JavTube.Models;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class ProviderIdsExtensions
{
    private const char Separator = '#';

    public static ProviderModel GetProviderModel(this IHasProviderIds instance, string id)
    {
        var pm = new ProviderModel();
        if (!instance.ProviderIds.Any())
            return pm;
        var v = instance.GetProviderId(id)?.Split(Separator);
        if (v?.Length > 0)
            pm.Provider = v[0];
        if (v?.Length > 1)
            pm.Id = v[1];
        return pm;
    }

    public static void SetProviderModel(this IHasProviderIds instance, string id, ProviderModel pm)
    {
        instance.SetProviderId(id, $"{pm.Provider}{Separator}{pm.Id}");
    }
}