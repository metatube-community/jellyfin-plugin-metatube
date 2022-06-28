namespace Jellyfin.Plugin.JavTube.Helpers;

public static class DictionaryHelper
{
    public static Dictionary<string, string> Deserialize(string text)
    {
        var dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

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
}