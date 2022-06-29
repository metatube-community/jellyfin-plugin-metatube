namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<(int index, T item)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (index, item));
    }

    public static IEnumerable<string> Substitute(this IEnumerable<string> source, Dictionary<string, string> table)
    {
        if (table?.Any() != true)
        {
            return source;
        }

        var target = new List<string>();

        foreach (var item in source ?? Enumerable.Empty<string>())
        {
            if (!table.TryGetValue(item, out var value))
                target.Add(item);
            else if (!string.IsNullOrWhiteSpace(value))
                target.Add(value);
        }

        return target;
    }
}