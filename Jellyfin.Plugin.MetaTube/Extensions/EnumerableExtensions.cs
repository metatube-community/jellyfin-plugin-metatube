namespace Jellyfin.Plugin.MetaTube.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<(int index, T item)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (index, item));
    }
}