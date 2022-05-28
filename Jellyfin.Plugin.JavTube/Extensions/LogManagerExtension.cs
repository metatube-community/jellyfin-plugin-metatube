#if __EMBY__
using MediaBrowser.Model.Logging;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class LogManagerExtension
{
    public static ILogger CreateLogger<T>(this ILogManager logManager)
    {
        return logManager.GetLogger($"{Constant.JavTube}.{typeof(T).Name}");
    }
}

#endif