#if __EMBY__
using System.Reflection;
using MediaBrowser.Model.Logging;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class LogManagerExtension
{
    public static ILogger CreateLogger<T>(this ILogManager instance)
    {
        return instance.GetLogger($"{Constant.JavTube}.{typeof(T).Name}");
    }
}

#endif