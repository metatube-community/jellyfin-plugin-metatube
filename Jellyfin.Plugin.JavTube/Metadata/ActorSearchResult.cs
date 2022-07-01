using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class ActorSearchResult : BaseInfo
{
    [JsonPropertyName("images")]
    public string[] Images { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}