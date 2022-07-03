using Jellyfin.Plugin.JavTube.Translation;
using Jellyfin.Plugin.JavTube.Utilities;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.JavTube.Configuration;

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

    public bool EnableActorSubstitution { get; set; } = false;

    public string ActorRawSubstitutionTable
    {
        get => _actorSubstitutionTable?.ToString();
        set => _actorSubstitutionTable = Table.Parse(value);
    }

    public Table GetActorSubstitutionTable()
    {
        return _actorSubstitutionTable;
    }

    private Table _actorSubstitutionTable;

    public bool EnableGenreSubstitution { get; set; } = false;

    public string GenreRawSubstitutionTable
    {
        get => _genreSubstitutionTable?.ToString();
        set => _genreSubstitutionTable = Table.Parse(value);
    }

    public Table GetGenreSubstitutionTable()
    {
        return _genreSubstitutionTable;
    }

    private Table _genreSubstitutionTable;

    #endregion
}