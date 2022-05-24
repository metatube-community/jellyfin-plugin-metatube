using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.JavTube.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        Server = "http://javtube-server:8080";
        Token = string.Empty;
    }

    /// <summary>
    ///     Gets or sets the JavTube server URL.
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    ///     Gets or sets the JavTube server API token.
    /// </summary>
    public string Token { get; set; }
}