using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class Response<T>
{
    [JsonPropertyName("data")]
    public T Data { get; set; }

    [JsonPropertyName("error")]
    public Error Error { get; set; }
}