using System.ComponentModel;

namespace Jellyfin.Plugin.MetaTube.Translation;

// Not marked [Flags]: Emby's options editor can't select or save a flags enum
// from a dropdown, so the value would come back blank.
public enum TranslationMode
{
    [Description("Disabled")]
    Disabled = 0,

    [Description("Title")]
    Title = 1,

    [Description("Summary")]
    Summary = 2,

    [Description("Title and Summary")]
    Both = 3
}
