using System.ComponentModel;

namespace Jellyfin.Plugin.MetaTube.Translation;

public enum TranslationEngine
{
    [Description("Baidu")]
    Baidu,

    [Description("Google")]
    Google,

    [Description("Google (Free)")]
    GoogleFree,

    [Description("DeepL (Free)")]
    DeepL,

    [Description("OpenAI")]
    OpenAi,

    [Description("OpenAI (Custom)")]
    OpenAiX
}