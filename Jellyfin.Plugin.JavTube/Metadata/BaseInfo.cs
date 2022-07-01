using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class BaseInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("provider")]
    public string Provider { get; set; }

    [JsonPropertyName("homepage")]
    public string Homepage { get; set; }
}