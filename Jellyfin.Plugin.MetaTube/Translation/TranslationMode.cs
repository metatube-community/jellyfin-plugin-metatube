using System.ComponentModel;

namespace Jellyfin.Plugin.MetaTube.Translation;

[Flags]
public enum TranslationMode
{
    [Description("Disabled")]
    Disabled = 0,

    [Description("Title")]
    Title = 1 << 0,

    [Description("Summary")]
    Summary = 1 << 1,

    [Description("Title and Summary")]
    Both = Title | Summary
}
