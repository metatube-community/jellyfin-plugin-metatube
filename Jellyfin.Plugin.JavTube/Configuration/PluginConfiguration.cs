using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.JavTube.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        Server = "https://api.javtube.internal";
        Token = string.Empty;
        TranslationMode = 0;
        TranslationEngine = "Baidu";
        BaiduAppId = string.Empty;
        BaiduAppKey = string.Empty;
        GoogleApiKey = string.Empty;
    }

    public string Server { get; set; }

    public string Token { get; set; }

    public int TranslationMode { get; set; }

    public string TranslationEngine { get; set; }

    public string BaiduAppId { get; set; }

    public string BaiduAppKey { get; set; }

    public string GoogleApiKey { get; set; }
}