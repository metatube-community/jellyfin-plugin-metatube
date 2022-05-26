using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using Jellyfin.Plugin.JavTube.Models;

namespace Jellyfin.Plugin.JavTube;

public class ApiClient
{
    private const string ActorMetadataApi = "/api/actor";
    private const string MovieMetadataApi = "/api/movie";
    private const string ActorSearchApi = "/api/search/actor";
    private const string MovieSearchApi = "/api/search/movie";
    private const string PrimaryImageApi = "/image/primary";
    private const string ThumbImageApi = "/image/thumb";
    private const string BackdropImageApi = "/image/backdrop";

    private static string ComposeUrl(string path, NameValueCollection nv)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        foreach (string key in nv) query.Add(key, nv.Get(key));

        // Build URL
        var uriBuilder = new UriBuilder(Plugin.Instance.Configuration.Server)
        {
            Path = path,
            Query = query.ToString() ?? string.Empty
        };
        return uriBuilder.ToString();
    }

    private static string ComposeMetadataApiUrl(string path, string id, string url, string provider, string language,
        bool lazy)
    {
        return ComposeUrl(path, new NameValueCollection
        {
            { "id", id },
            { "provider", provider },
            { "url", url },
            { "lang", language },
            { "lazy", lazy ? "1" : "0" }
        });
    }

    private static string ComposeSearchApiUrl(string path, string keyword, string provider, bool lazy)
    {
        return ComposeUrl(path, new NameValueCollection
        {
            { "keyword", keyword },
            { "provider", provider },
            { "lazy", lazy ? "1" : "0" }
        });
    }

    private static string ComposeImageApiUrl(string path, string id, string provider, string url, float position,
        bool auto)
    {
        return ComposeUrl(path, new NameValueCollection
        {
            { "id", id },
            { "provider", provider },
            { "url", url },
            { "pos", position.ToString("R") },
            { "auto", auto ? "1" : "0" }
        });
    }

    public static string GetPrimaryImageApiUrl(string id, string provider, float position = -1)
    {
        return ComposeImageApiUrl(PrimaryImageApi, id, provider, string.Empty, position, false);
    }

    public static string GetPrimaryImageApiUrl(string id, string provider, string url, float position = -1,
        bool auto = false)
    {
        return ComposeImageApiUrl(PrimaryImageApi, id, provider, url, position, auto);
    }

    public static string GetThumbImageApiUrl(string id, string provider)
    {
        return ComposeImageApiUrl(ThumbImageApi, id, provider, string.Empty, -1, false);
    }

    public static string GetThumbImageApiUrl(string id, string provider, string url, float position = -1,
        bool auto = false)
    {
        return ComposeImageApiUrl(ThumbImageApi, id, provider, url, position, auto);
    }

    public static string GetBackdropImageApiUrl(string id, string provider)
    {
        return ComposeImageApiUrl(BackdropImageApi, id, provider, string.Empty, -1, false);
    }

    public static string GetBackdropImageApiUrl(string id, string provider, string url, float position = -1,
        bool auto = false)
    {
        return ComposeImageApiUrl(BackdropImageApi, id, provider, url, position, auto);
    }

    public static async Task<ActorMetadata> GetActorMetadata(string id, string provider,
        CancellationToken cancellationToken)
    {
        return await GetActorMetadata(id, provider, string.Empty, true, cancellationToken);
    }

    public static async Task<ActorMetadata> GetActorMetadata(string url,
        CancellationToken cancellationToken)
    {
        return await GetActorMetadata(string.Empty, string.Empty, url, true, cancellationToken);
    }

    public static async Task<ActorMetadata> GetActorMetadata(string id, string provider, string url, bool lazy,
        CancellationToken cancellationToken)
    {
        var apiUrl = ComposeMetadataApiUrl(ActorMetadataApi, id, url, provider, string.Empty, lazy);
        return await GetDataFromApi<ActorMetadata>(apiUrl, cancellationToken);
    }

    public static async Task<MovieMetadata> GetMovieMetadata(string url, CancellationToken cancellationToken)
    {
        return await GetMovieMetadata(string.Empty, string.Empty, url, true, string.Empty, cancellationToken);
    }

    public static async Task<MovieMetadata> GetMovieMetadata(string url, string language,
        CancellationToken cancellationToken)
    {
        return await GetMovieMetadata(string.Empty, string.Empty, url, true, language, cancellationToken);
    }

    public static async Task<MovieMetadata> GetMovieMetadata(string id, string provider, string url, bool lazy,
        string language, CancellationToken cancellationToken)
    {
        var apiUrl = ComposeMetadataApiUrl(MovieMetadataApi, id, provider, url, language, lazy);
        return await GetDataFromApi<MovieMetadata>(apiUrl, cancellationToken);
    }

    public static async Task<IEnumerable<ActorSearchResult>> SearchActor(string keyword, string provider,
        CancellationToken cancellationToken)
    {
        return await SearchActor(keyword, provider, false, cancellationToken);
    }

    public static async Task<IEnumerable<ActorSearchResult>> SearchActor(string keyword, string provider, bool lazy,
        CancellationToken cancellationToken)
    {
        var apiUrl = ComposeSearchApiUrl(ActorSearchApi, keyword, provider, lazy);
        return await GetDataFromApi<List<ActorSearchResult>>(apiUrl, cancellationToken);
    }

    public static async Task<IEnumerable<MovieSearchResult>> SearchMovie(string keyword, string provider,
        CancellationToken cancellationToken)
    {
        return await SearchMovie(keyword, provider, false, cancellationToken);
    }

    public static async Task<IEnumerable<MovieSearchResult>> SearchMovie(string keyword, string provider, bool lazy,
        CancellationToken cancellationToken)
    {
        var apiUrl = ComposeSearchApiUrl(MovieSearchApi, keyword, provider, lazy);
        return await GetDataFromApi<List<MovieSearchResult>>(apiUrl, cancellationToken);
    }

    private static async Task<T> GetDataFromApi<T>(string url, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var httpClient = new HttpClient();
        // Set default timeout: 5 minutes.
        httpClient.Timeout = TimeSpan.FromSeconds(300);
        // Set Accept JSON header.
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        // Set User-Agent header.
        httpClient.DefaultRequestHeaders.Add("User-Agent", Constant.UserAgent);
        // Set Authorization API Token.
        if (!string.IsNullOrWhiteSpace(Plugin.Instance.Configuration.Token))
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Plugin.Instance.Configuration.Token);

        var response = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStreamAsync(cancellationToken));
    }
}