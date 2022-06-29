namespace Jellyfin.Plugin.JavTube.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<(int index, T item)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (index, item));
    }

    public static IEnumerable<string> Substitute(this IEnumerable<string> source, Dictionary<string, string> table)
    {
        var target = new List<string>();

        foreach (var item in source ?? Enumerable.Empty<string>())
        {
            if (!table.TryGetValue(item, out var value))
                target.Add(item); // Add original item.
            else if (!string.IsNullOrWhiteSpace(value))
                target.Add(value); // Add replaced item.
            // Ignore deleted item.
        }

        return target;
    }
}