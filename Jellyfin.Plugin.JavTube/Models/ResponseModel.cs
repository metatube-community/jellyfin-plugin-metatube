using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Models;

public class ResponseModel<TModel>
{
    [JsonPropertyName("data")]
    public TModel Data { get; set; }

    [JsonPropertyName("error")]
    public ErrorModel Error { get; set; }
}

public class ErrorModel
{
    [JsonPropertyName("code")]
    public int Code { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }
}