namespace Jellyfin.Plugin.JavTube;

public static class Constant
{
    public const string JavTube = "JavTube";

    public const string Description = "Just Another Video Tube";

    public const string Guid = "df87283d-7224-4f9c-a448-3433d9cf278a";

    public const string Rating = "JP-18+";

    public static readonly string UserAgent = $"{typeof(Plugin).Namespace}/{Plugin.Instance.Version}";
}