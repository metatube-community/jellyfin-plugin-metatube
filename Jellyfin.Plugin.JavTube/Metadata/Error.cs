using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class Error
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}