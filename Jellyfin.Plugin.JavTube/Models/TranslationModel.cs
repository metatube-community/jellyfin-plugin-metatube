using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Models;

public class TranslationModel
{
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }

    [JsonPropertyName("translated_text")]
    public string TranslatedText { get; set; }
}