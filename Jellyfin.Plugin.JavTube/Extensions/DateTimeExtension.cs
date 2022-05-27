namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class DateTimeExtension
{
    public static DateTime? ValidDateTime(this DateTime instance)
    {
        return instance.Year > 1 ? instance : null;
    }
}