using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.JavTube.Configuration;

public enum TranslationMode
{
    Disabled,
    Title,
    Summary,
    Both
}

public enum TranslationEngine
{
    Baidu,
    Google
}

public class PluginConfiguration : BasePluginConfiguration
{
    #region General

    public string Server { get; set; } = "https://api.javtube.internal";

    public string Token { get; set; } = string.Empty;

    public bool EnableCollection { get; set; } = false;

    public bool EnableRating { get; set; } = true;

    public bool EnableTrailer { get; set; } = false;

    #endregion

    #region Translation

    public TranslationMode TranslationMode { get; set; } = TranslationMode.Disabled;

    public TranslationEngine TranslationEngine { get; set; } = TranslationEngine.Baidu;

    public string BaiduAppId { get; set; } = string.Empty;

    public string BaiduAppKey { get; set; } = string.Empty;

    public string GoogleApiKey { get; set; } = string.Empty;

    #endregion

    #region Substitution

    #region Actors

    public bool EnableActorSubstitution { get; set; } = false;

    public string ActorRawSubstitutionTable
    {
        get => TableSerializer.Serialize(_actorSubstitutionTable);
        set => _actorSubstitutionTable = TableSerializer.Deserialize(value);
    }

    public Dictionary<string, string> GetActorSubstitutionTable()
    {
        return _actorSubstitutionTable;
    }

    private Dictionary<string, string> _actorSubstitutionTable;

    #endregion

    #region Genres

    public bool EnableGenreSubstitution { get; set; } = false;

    public string GenreRawSubstitutionTable
    {
        get => TableSerializer.Serialize(_genreSubstitutionTable);
        set => _genreSubstitutionTable = TableSerializer.Deserialize(value);
    }

    public Dictionary<string, string> GetGenreSubstitutionTable()
    {
        return _genreSubstitutionTable;
    }

    private Dictionary<string, string> _genreSubstitutionTable;

    #endregion

    #endregion

    #region TableSerializer

    private class TableSerializer
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

        public static string Serialize(Dictionary<string, string> table)
        {
            return table?.Any() != true
                ? string.Empty
                : string.Join('\n',
                    table.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Key))
                        .Select(kvp => $"{kvp.Key?.Trim()}={kvp.Value?.Trim()}"));
        }
    }

    #endregion
}