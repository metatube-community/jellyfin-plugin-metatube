using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class ResponseInfo<T>
{
    [JsonPropertyName("data")]
    public T Data { get; set; }

    [JsonPropertyName("error")]
    public ErrorInfo Error { get; set; }

    public class ErrorInfo
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}