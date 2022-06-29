#if !__EMBY__
using MediaBrowser.Controller.Entities.Movies;

namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class MovieExtensions
{
    public static void AddCollection(this Movie movie, string name)
    {
        movie.CollectionName = name;
    }
}
#endif