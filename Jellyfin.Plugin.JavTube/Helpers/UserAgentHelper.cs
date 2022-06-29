namespace Jellyfin.Plugin.JavTube.Helpers;

internal static class UserAgentHelper
{
    public static string Default => $"{Plugin.Instance.Name}/{Plugin.Instance.Version}";
}