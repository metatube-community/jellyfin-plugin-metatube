using Jellyfin.Plugin.JavTube.Extensions;

namespace Jellyfin.Plugin.JavTube;

public class ProviderId
{
    public string Provider { get; set; }

    public string Id { get; set; }

    public double? Position { get; set; }

    public bool? UpdateInfo { get; set; }

    #region Serializer

    private const char Separator = ':';

    public static ProviderId Deserialize(string rawPid)
    {
        var values = rawPid?.Split(Separator);
        return new ProviderId
        {
            Provider = values?.Length > 0 ? values[0] : string.Empty,
            Id = values?.Length > 1 ? values[1] : string.Empty,
            Position = values?.Length > 2 ? values[2].ToDouble() : null,
            UpdateInfo = values?.Length > 3 ? values[3].ToBool() : null
        };
    }

    public string Serialize()
    {
        var pid = this;
        var values = new List<string>
        {
            pid.Provider, pid.Id
        };
        if (pid.Position.HasValue) values.Add(pid.Position.ToString());
        if (pid.UpdateInfo.HasValue) values.Add((values.Count == 2 ? ":" : string.Empty) + pid.UpdateInfo);
        return string.Join(Separator, values);
    }

    #endregion
}