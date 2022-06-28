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
    #region Collection

    public bool EnableCollection { get; set; } = false;

    #endregion

    #region Rating

    public bool EnableRating { get; set; } = true;

    #endregion

    #region Trailer

    public bool EnableTrailer { get; set; } = false;

    #endregion

    #region General

    public string Server { get; set; } = "https://api.javtube.internal";

    public string Token { get; set; } = string.Empty;

    #endregion

    #region Translation

    public TranslationMode TranslationMode { get; set; } = TranslationMode.Disabled;

    public TranslationEngine TranslationEngine { get; set; } = TranslationEngine.Baidu;

    public string BaiduAppId { get; set; } = string.Empty;

    public string BaiduAppKey { get; set; } = string.Empty;

    public string GoogleApiKey { get; set; } = string.Empty;

    #endregion

    #region Genre
    
    public bool EnableGenreSubstitution { get; set; } = true;

    public string GenreSubstitutionTable { get; set; } = DefaultGenreSubstitutionTable;

    private static string DefaultGenreSubstitutionTable =>
        @"HD=
FHD=
4K=
5K=
720p=
1080p=
60fps=";

    #endregion
}