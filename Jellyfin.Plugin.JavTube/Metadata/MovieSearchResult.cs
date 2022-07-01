using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class MovieSearchResult : ProviderInfo
{
    [JsonPropertyName("cover_url")]
    public string CoverUrl { get; set; }

    [JsonPropertyName("number")]
    public string Number { get; set; }

    [JsonPropertyName("release_date")]
    public DateTime ReleaseDate { get; set; }

    [JsonPropertyName("score")]
    public float Score { get; set; }

    [JsonPropertyName("thumb_url")]
    public string ThumbUrl { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}