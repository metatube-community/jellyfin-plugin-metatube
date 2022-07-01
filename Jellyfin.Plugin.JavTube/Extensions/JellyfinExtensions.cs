#if !__EMBY__
#pragma warning disable CA2254

using MediaBrowser.Controller.Entities.Movies;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.JavTube.Extensions;

public static class JellyfinExtensions
{
    #region LoggerExtensions

    public static void Debug(this ILogger logger, string message, params object[] args)
    {
        logger.LogDebug(message, args);
    }

    public static void Info(this ILogger logger, string message, params object[] args)
    {
        logger.LogInformation(message, args);
    }

    public static void Warn(this ILogger logger, string message, params object[] args)
    {
        logger.LogWarning(message, args);
    }

    public static void Error(this ILogger logger, string message, params object[] args)
    {
        logger.LogError(message, args);
    }

    #endregion

    #region MovieExtensions

    public static void AddCollection(this Movie movie, string name)
    {
        movie.CollectionName = name;
    }

    #endregion
}

#pragma warning restore CA2254
#endif