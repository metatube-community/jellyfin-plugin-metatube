namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class DateTimeExtension
{
    public static DateTime? ValidDateTime(this DateTime dateTime)
    {
        return dateTime.Year > 1 ? dateTime : null;
    }
}