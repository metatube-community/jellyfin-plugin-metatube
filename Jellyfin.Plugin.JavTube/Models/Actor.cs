using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Models;

public class ActorSearchResult
{
    [JsonPropertyName("homepage")] public string Homepage { get; set; }

    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("images")] public string[] Images { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("provider")] public string Provider { get; set; }
}

public class ActorMetadata : ActorSearchResult
{
    [JsonPropertyName("aliases")] public string[] Aliases { get; set; }

    [JsonPropertyName("birthday")] public DateTime? Birthday { get; set; }

    [JsonPropertyName("blood_type")] public string BloodType { get; set; }

    [JsonPropertyName("cup_size")] public string CupSize { get; set; }

    [JsonPropertyName("debut_date")] public DateTime DebutDate { get; set; }

    [JsonPropertyName("height")] public int Height { get; set; }

    [JsonPropertyName("hobby")] public string Hobby { get; set; }

    [JsonPropertyName("measurements")] public string Measurements { get; set; }

    [JsonPropertyName("nationality")] public string Nationality { get; set; }

    [JsonPropertyName("summary")] public string Summary { get; set; }
}