using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Metadata;

public class MovieInfo : MovieSearchResult
{
    [JsonPropertyName("actors")]
    public string[] Actors { get; set; }

    [JsonPropertyName("big_cover_url")]
    public string BigCoverUrl { get; set; }

    [JsonPropertyName("big_thumb_url")]
    public string BigThumbUrl { get; set; }

    [JsonPropertyName("director")]
    public string Director { get; set; }

    [JsonPropertyName("genres")]
    public string[] Genres { get; set; }

    [JsonPropertyName("maker")]
    public string Maker { get; set; }

    [JsonPropertyName("preview_images")]
    public string[] PreviewImages { get; set; }

    [JsonPropertyName("preview_video_hls_url")]
    public string PreviewVideoHlsUrl { get; set; }

    [JsonPropertyName("preview_video_url")]
    public string PreviewVideoUrl { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("runtime")]
    public int Runtime { get; set; }

    [JsonPropertyName("series")]
    public string Series { get; set; }

    [JsonPropertyName("summary")]
    public string Summary { get; set; }
}