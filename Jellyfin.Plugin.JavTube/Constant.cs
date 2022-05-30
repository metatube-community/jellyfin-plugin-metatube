namespace Jellyfin.Plugin.JavTube;

public static class Constant
{
    // public const string JavTube = "JavTube";

    // public const string Rating = "JP-18+";

    public static readonly string UserAgent = $"{typeof(Plugin).Namespace}/{Plugin.Instance.Version}";
}