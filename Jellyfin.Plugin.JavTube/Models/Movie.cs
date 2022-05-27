using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Models;

public class MovieSearchResult : ProviderInfo
{
    [JsonPropertyName("cover_url")] public string CoverUrl { get; set; }

    [JsonPropertyName("number")] public string Number { get; set; }

    [JsonPropertyName("release_date")] public DateTime ReleaseDate { get; set; }

    [JsonPropertyName("score")] public float Score { get; set; }

    [JsonPropertyName("thumb_url")] public string ThumbUrl { get; set; }

    [JsonPropertyName("title")] public string Title { get; set; }
}

public class MovieMetadata : MovieSearchResult
{
    [JsonPropertyName("actors")] public string[] Actors { get; set; }

    [JsonPropertyName("big_cover_url")] public string BigCoverUrl { get; set; }

    [JsonPropertyName("big_thumb_url")] public string BigThumbUrl { get; set; }

    [JsonPropertyName("director")] public string Director { get; set; }

    [JsonPropertyName("maker")] public string Maker { get; set; }

    [JsonPropertyName("preview_images")] public string[] PreviewImages { get; set; }

    [JsonPropertyName("preview_video_hls_url")]
    public string PreviewVideoHlsUrl { get; set; }

    [JsonPropertyName("preview_video_url")]
    public string PreviewVideoUrl { get; set; }

    [JsonPropertyName("publisher")] public string Publisher { get; set; }

    [JsonPropertyName("runtime")] public int Runtime { get; set; }

    [JsonPropertyName("series")] public string Series { get; set; }

    [JsonPropertyName("summary")] public string Summary { get; set; }

    [JsonPropertyName("tags")] public string[] Tags { get; set; }
}