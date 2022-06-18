namespace Jellyfin.Plugin.JavTube.Helpers;

public static class HttpClientHelper
{
    public static string UserAgent => $"{Plugin.Instance.Name}/{Plugin.Instance.Version}";
}