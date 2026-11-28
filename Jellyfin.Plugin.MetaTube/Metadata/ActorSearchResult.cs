using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.MetaTube.Metadata;

public class ActorSearchResult : ProviderInfo
{
    [JsonPropertyName("images")]
    public string[] Images { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}