namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class StringExtensions
{
    public static bool? ToBool(this string s)
    {
        switch (s)
        {
            case "1":
            case "t":
            case "T":
            case "true":
            case "True":
            case "TRUE":
                return true;
            case "0":
            case "f":
            case "F":
            case "false":
            case "False":
            case "FALSE":
                return false;
        }

        return null;
    }

    public static double? ToDouble(this string s)
    {
        return double.TryParse(s, out var result) ? result : null;
    }
}