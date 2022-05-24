#if __EMBY__
using System.Reflection;
using MediaBrowser.Model.Logging;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class LogManagerExtension
{
    public static ILogger CreateLogger<T>(this ILogManager factory)
    {
        return factory.GetLogger(Format(typeof(T)));
    }

    private static string Format(MemberInfo type)
    {
        return $"{Constant.JavTube}.{type.Name}";
    }
}

#endif