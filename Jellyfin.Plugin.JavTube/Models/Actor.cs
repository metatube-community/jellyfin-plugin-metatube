using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Models;

public class ActorSearchResult
{
    [JsonPropertyName("homepage")] public string Homepage;

    [JsonPropertyName("id")] public string Id;

    [JsonPropertyName("images")] public string[] Images;

    [JsonPropertyName("name")] public string Name;

    [JsonPropertyName("provider")] public string Provider;
}

public class ActorInfo : ActorSearchResult
{
    [JsonPropertyName("aliases")] public string[] Aliases;

    [JsonPropertyName("birthday")] public DateTime? Birthday;

    [JsonPropertyName("blood_type")] public string BloodType;

    [JsonPropertyName("cup_size")] public string CupSize;

    [JsonPropertyName("debut_date")] public DateTime? DebutDate;

    [JsonPropertyName("height")] public int Height;

    [JsonPropertyName("hobby")] public string Hobby;

    [JsonPropertyName("measurements")] public string Measurements;

    [JsonPropertyName("nationality")] public string Nationality;

    [JsonPropertyName("summary")] public string Summary;
}