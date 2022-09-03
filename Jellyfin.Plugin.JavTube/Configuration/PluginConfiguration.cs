using Jellyfin.Plugin.JavTube.Helpers;
using Jellyfin.Plugin.JavTube.Translation;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.JavTube.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    #region Image

    public double PrimaryImageRatio { get; set; } = -1;

    #endregion

    #region General

    public string Server { get; set; } = "https://api.javtube.internal";

    public string Token { get; set; } = string.Empty;

    public bool EnableCollections { get; set; } = false;

    public bool EnableDirectors { get; set; } = true;

    public bool EnableRatings { get; set; } = true;

    public bool EnableTrailers { get; set; } = false;

    public bool EnableRealActorNames { get; set; } = false;

    #endregion

    #region Template

    public string NameTemplate { get; set; } = "{number} {title}";

    public string TaglineTemplate { get; set; } = "配信開始日 {date}";

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
        get => _actorSubstitutionSubstitutionTable?.ToString();
        set => _actorSubstitutionSubstitutionTable = SubstitutionTable.Parse(value);
    }

    public SubstitutionTable GetActorSubstitutionTable()
    {
        return _actorSubstitutionSubstitutionTable;
    }

    private SubstitutionTable _actorSubstitutionSubstitutionTable;

    public bool EnableGenreSubstitution { get; set; } = false;

    public string GenreRawSubstitutionTable
    {
        get => _genreSubstitutionSubstitutionTable?.ToString();
        set => _genreSubstitutionSubstitutionTable = SubstitutionTable.Parse(value);
    }

    public SubstitutionTable GetGenreSubstitutionTable()
    {
        return _genreSubstitutionSubstitutionTable;
    }

    private SubstitutionTable _genreSubstitutionSubstitutionTable;

    #endregion
}