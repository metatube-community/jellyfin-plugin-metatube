using Jellyfin.Plugin.JavTube.Helpers;
using Jellyfin.Plugin.JavTube.Translation;
using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.JavTube.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    #region Image

    public double PrimaryImageRatio { get; set; } = -1;

    public int DefaultImageQuality { get; set; } = 90;

    #endregion

    #region Badge

    public bool EnableBadges { get; set; } = false;

    public string BadgeUrl { get; set; } = "zimu.png";

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

    public string DeeplApiKey { get; set; } = string.Empty;

    #endregion

    #region Provider

    public bool EnableMovieProviderFilter { get; set; } = false;

    public string RawMovieProviderFilter
    {
        get => _movieProviderFilter?.Any() == true ? string.Join(',', _movieProviderFilter) : string.Empty;
        set => _movieProviderFilter = value?.Split(',').Select(s => s.Trim()).Where(s => s.Any())
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    public List<string> GetMovieProviderFilter()
    {
        return _movieProviderFilter;
    }

    private List<string> _movieProviderFilter;

    #endregion

    #region Substitution

    public bool EnableTitleSubstitution { get; set; } = false;

    public string TitleRawSubstitutionTable
    {
        get => _titleSubstitutionTable?.ToString();
        set => _titleSubstitutionTable = SubstitutionTable.Parse(value);
    }

    public SubstitutionTable GetTitleSubstitutionTable()
    {
        return _titleSubstitutionTable;
    }

    private SubstitutionTable _titleSubstitutionTable;

    public bool EnableActorSubstitution { get; set; } = false;

    public string ActorRawSubstitutionTable
    {
        get => _actorSubstitutionTable?.ToString();
        set => _actorSubstitutionTable = SubstitutionTable.Parse(value);
    }

    public SubstitutionTable GetActorSubstitutionTable()
    {
        return _actorSubstitutionTable;
    }

    private SubstitutionTable _actorSubstitutionTable;

    public bool EnableGenreSubstitution { get; set; } = false;

    public string GenreRawSubstitutionTable
    {
        get => _genreSubstitutionTable?.ToString();
        set => _genreSubstitutionTable = SubstitutionTable.Parse(value);
    }

    public SubstitutionTable GetGenreSubstitutionTable()
    {
        return _genreSubstitutionTable;
    }

    private SubstitutionTable _genreSubstitutionTable;

    #endregion
}