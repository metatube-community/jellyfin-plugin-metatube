using Jellyfin.Plugin.JavTube.Models;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class ProviderIdModelExtensions
{
    private const char Separator = ':';

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

    public static ProviderIdModel Deserialize(string rawPid)
    {
        var providerIds = rawPid?.Split(Separator);
        return new ProviderIdModel
        {
            Provider = providerIds?.Length > 0 ? providerIds[0] : string.Empty,
            Id = providerIds?.Length > 1 ? providerIds[1] : string.Empty,
            Position = providerIds?.Length > 2 ? ParseDouble(providerIds[2]) : null,
            UpdateInfo = providerIds?.Length > 3 ? ParseBool(providerIds[3]) : null
        };
    }

    public static string Serialize(this ProviderIdModel pid)
    {
        var values = new List<string>
        {
            pid.Provider, pid.Id
        };
        if (pid.Position.HasValue) values.Add(pid.Position.ToString());
        if (pid.UpdateInfo.HasValue) values.Add(pid.UpdateInfo.ToString());
        return string.Join(Separator, values);
    }
}