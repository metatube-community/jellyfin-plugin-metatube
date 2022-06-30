namespace Jellyfin.Plugin.JavTube;

public class ProviderId
{
    public string Provider { get; set; }

    public string Id { get; set; }

    public double? Position { get; set; }

    public bool? UpdateInfo { get; set; }

    public string Serialize()
    {
        return ProviderIdSerializer.Serialize(this);
    }
}

public static class ProviderIdSerializer
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

    public static ProviderId Deserialize(string rawPid)
    {
        var values = rawPid?.Split(Separator);
        return new ProviderId
        {
            Provider = values?.Length > 0 ? values[0] : string.Empty,
            Id = values?.Length > 1 ? values[1] : string.Empty,
            Position = values?.Length > 2 ? ParseDouble(values[2]) : null,
            UpdateInfo = values?.Length > 3 ? ParseBool(values[3]) : null
        };
    }

    public static string Serialize(ProviderId pid)
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