using Jellyfin.Plugin.JavTube.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
#if __EMBY__
using MediaBrowser.Model.Drawing;
#endif

namespace Jellyfin.Plugin.JavTube;

#if __EMBY__
public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages, IHasThumbImage
#else
public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
#endif
{
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths,
        xmlSerializer)
    {
        Instance = this;
    }

    public override string Name => "JavTube";

    public override string Description => "Just Another Video Tube";

    public override Guid Id => Guid.Parse("df87283d-7224-4f9c-a448-3433d9cf278a");

    public static Plugin Instance { get; private set; }

    public IEnumerable<PluginPageInfo> GetPages()
    {
        return new[]
        {
            new PluginPageInfo
            {
                Name = Name,
                EmbeddedResourcePath = $"{GetType().Namespace}.Configuration.configPage.html"
            }
        };
    }

#if __EMBY__
    public Stream GetThumbImage()
    {
        var type = GetType();
        return type.Assembly.GetManifestResourceStream($"{GetType().Namespace}.thumb.png");
    }

    public ImageFormat ThumbImageFormat => ImageFormat.Png;
#endif
}