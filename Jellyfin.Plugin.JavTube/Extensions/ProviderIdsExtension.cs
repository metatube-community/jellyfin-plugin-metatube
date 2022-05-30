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
            Id = providerIds?.Length > 1 ? providerIds[1] : string.Empty,
            Position = providerIds?.Length > 2 ? ParseDouble(providerIds[2]) : null,
            UpdateInfo = providerIds?.Length > 3 ? ParseBool(providerIds[3]) : null
        };
    }

    public static void SetProviderIdModel(this IHasProviderIds instance, string name, ProviderIdModel pid)
    {
        var values = new List<string>
        {
            pid.Provider, pid.Id
        };
        if (pid.Position.HasValue) values.Add(pid.Position.ToString());
        if (pid.UpdateInfo.HasValue) values.Add(pid.UpdateInfo.ToString());
        instance.SetProviderId(name, string.Join(Separator, values));
    }

    private static double? ParseDouble(string s)
    {
        try
        {
            return double.Parse(s);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static bool? ParseBool(string s)
    {
        switch (s)
        {
            case "1":
            case "t":
            case "T":
            case "true":
            case "True":
            case "TRUE":
                return true;
            case "0":
            case "f":
            case "F":
            case "false":
            case "False":
            case "FALSE":
                return false;
        }

        return null;
    }
}