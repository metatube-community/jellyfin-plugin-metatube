namespace Jellyfin.Plugin.JavTube.Common;

public class Table : Dictionary<string, string>
{
    private Table() : base(StringComparer.OrdinalIgnoreCase)
    {
    }

    public static Table Parse(string text)
    {
        var dictionary = new Table();

        var reader = new StringReader(text ?? string.Empty);
        while (reader.ReadLine() is { } line)
        {
            var kvp = line.Split('=', 2).Select(s => s.Trim()).ToList();
            if (string.IsNullOrWhiteSpace(kvp.First()))
                continue;
            dictionary[kvp[0]] = kvp.Count switch
            {
                1 => null,
                2 => kvp[1],
                _ => dictionary[kvp[0]]
            };
        }

        return dictionary;
    }

    public override string ToString()
    {
        var table = this;
        return table.Any() != true
            ? string.Empty
            : string.Join('\n',
                table.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                    .Select(kvp => $"{kvp.Key?.Trim()}={kvp.Value?.Trim()}"));
    }
}