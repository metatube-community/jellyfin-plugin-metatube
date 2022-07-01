using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class ResponseInfo<T>
{
    [JsonPropertyName("data")]
    public T Data { get; set; }

    [JsonPropertyName("error")]
    public ErrorInfo Error { get; set; }
}