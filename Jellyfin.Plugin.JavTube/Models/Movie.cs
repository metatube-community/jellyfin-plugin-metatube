using System.Text.Json.Serialization;

namespace Jellyfin.Plugin.JavTube.Models;

public class MovieSearchResult
{
    [JsonPropertyName("cover_url")] public string CoverUrl;

    [JsonPropertyName("homepage")] public string Homepage;

    [JsonPropertyName("id")] public string Id;

    [JsonPropertyName("number")] public string Number;

    [JsonPropertyName("provider")] public string Provider;

    [JsonPropertyName("release_date")] public DateTime ReleaseDate;

    [JsonPropertyName("score")] public float Score;

    [JsonPropertyName("thumb_url")] public string ThumbUrl;

    [JsonPropertyName("title")] public string Title;
}

public class MovieMetadata : MovieSearchResult
{
    [JsonPropertyName("actors")] public string[] Actors;

    [JsonPropertyName("big_cover_url")] public string BigCoverUrl;

    [JsonPropertyName("big_thumb_url")] public string BigThumbUrl;

    [JsonPropertyName("director")] public string Director;

    [JsonPropertyName("maker")] public string Maker;

    [JsonPropertyName("preview_images")] public string[] PreviewImages;

    [JsonPropertyName("preview_video_hls_url")]
    public string PreviewVideoHlsUrl;

    [JsonPropertyName("preview_video_url")]
    public string PreviewVideoUrl;

    [JsonPropertyName("publisher")] public string Publisher;

    [JsonPropertyName("runtime")] public int Runtime;

    [JsonPropertyName("series")] public string Series;

    [JsonPropertyName("summary")] public string Summary;

    [JsonPropertyName("tags")] public string[] Tags;
}