using Jellyfin.Plugin.MetaTube.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
#if __EMBY__
using MediaBrowser.Model.Drawing;
#endif

namespace Jellyfin.Plugin.MetaTube;

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

    public override string Name => "MetaTube";

    public override string Description => "Metadata Tube Plugin for Jellyfin/Emby";

    public override Guid Id => Guid.Parse("01cc53ec-c415-4108-bbd4-a684a9801a32");

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
        return GetType().Assembly.GetManifestResourceStream($"{GetType().Namespace}.thumb.png");
    }

    public ImageFormat ThumbImageFormat => ImageFormat.Png;
#endif
}