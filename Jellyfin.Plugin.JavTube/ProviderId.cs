namespace Jellyfin.Plugin.JavTube;

public class ProviderId
{
    public string Provider { get; set; }

    public string Id { get; set; }

    public double? Position { get; set; }

    public bool? Update { get; set; }

    public static ProviderId Parse(string rawPid)
    {
        var values = rawPid?.Split(':');
        return new ProviderId
        {
            Provider = values?.Length > 0 ? values[0] : string.Empty,
            Id = values?.Length > 1 ? values[1] : string.Empty,
            Position = values?.Length > 2 ? ToDouble(values[2]) : null,
            Update = values?.Length > 3 ? ToBool(values[3]) : null
        };
    }

    public override string ToString()
    {
        var pid = this;
        var values = new List<string>
        {
            pid.Provider, pid.Id
        };
        if (pid.Position.HasValue) values.Add(pid.Position.ToString());
        if (pid.Update.HasValue) values.Add((values.Count == 2 ? ":" : string.Empty) + pid.Update);
        return string.Join(':', values);
    }

    private static bool? ToBool(string s)
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

    private static double? ToDouble(string s)
    {
        return double.TryParse(s, out var result) ? result : null;
    }
}