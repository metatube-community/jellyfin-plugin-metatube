namespace Jellyfin.Plugin.JavTube;

public static class Constant
{
    public static readonly string UserAgent = $"{typeof(Plugin).Namespace}/{Plugin.Instance.Version}";
}