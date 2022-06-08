namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class DateTimeExtensions
{
    public static DateTime? TryGetValidDateTime(this DateTime dateTime)
    {
        return dateTime.Year > 1 ? dateTime : null;
    }

    public static int? TryGetValidYear(this DateTime dateTime)
    {
        return dateTime.TryGetValidDateTime()?.Year;
    }
}