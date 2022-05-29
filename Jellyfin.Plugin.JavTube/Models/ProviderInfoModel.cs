using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Models;

public class ProviderIdModel
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("provider")] public string Provider { get; set; }
}

public class ProviderInfoModel : ProviderIdModel
{
    [JsonPropertyName("homepage")] public string Homepage { get; set; }
}