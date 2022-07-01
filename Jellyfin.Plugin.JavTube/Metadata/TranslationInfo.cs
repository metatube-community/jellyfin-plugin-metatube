using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class TranslationInfo
{
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }

    [JsonPropertyName("translated_text")]
    public string TranslatedText { get; set; }
}