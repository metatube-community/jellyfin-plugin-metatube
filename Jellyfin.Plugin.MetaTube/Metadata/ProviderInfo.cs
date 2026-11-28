using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.MetaTube.Metadata;

public class ProviderInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("provider")]
    public string Provider { get; set; }

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; }
}