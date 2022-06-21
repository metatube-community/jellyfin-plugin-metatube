namespace Jellyfin.Plugin.JavTube.Helpers;

public static class UserAgentHelper
{
    public static string Default => $"{Plugin.Instance.Name}/{Plugin.Instance.Version}";
}