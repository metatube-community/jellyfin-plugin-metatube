using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using Jellyfin.Plugin.JavTube.Configuration;
using Jellyfin.Plugin.JavTube.Models;

namespace Jellyfin.Plugin.JavTube;

public class ApiClient
{
    private const string ActorInfoApi = "/api/actor";
    private const string MovieInfoApi = "/api/movie";
    private const string ActorSearchApi = "/api/search/actor";
    private const string MovieSearchApi = "/api/search/movie";
    private const string PrimaryImageApi = "/image/primary";
    private const string ThumbImageApi = "/image/thumb";
    private const string BackdropImageApi = "/image/backdrop";

    private static PluginConfiguration Config => Plugin.Instance?.Configuration ?? new PluginConfiguration();

    private static string ComposeUrl(string path, NameValueCollection nv)
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        foreach (string key in nv) query.Add(key, nv.Get(key));

        // Build URL
        var uriBuilder = new UriBuilder(Config.Server)
        {
            Path = path,
            Query = query.ToString() ?? string.Empty
        };
        return uriBuilder.ToString();
    }

    private static string ComposeInfoApiUrl(string path, string id, string provider, string language, bool lazy)
    {
        return ComposeUrl(path, new NameValueCollection
        {
            { "id", id },
            { "provider", provider },
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

    public static async Task<ActorInfo> GetActorInfo(string id, string provider,
        CancellationToken cancellationToken)
    {
        return await GetActorInfo(id, provider, true, cancellationToken);
    }

    public static async Task<ActorInfo> GetActorInfo(string id, string provider, bool lazy,
        CancellationToken cancellationToken)
    {
        var url = ComposeInfoApiUrl(ActorInfoApi, id, provider, string.Empty, lazy);
        return await GetDataFromApi<ActorInfo>(url, cancellationToken);
    }

    public static async Task<MovieInfo> GetMovieInfo(string id, string provider, CancellationToken cancellationToken)
    {
        return await GetMovieInfo(id, provider, true, string.Empty, cancellationToken);
    }

    public static async Task<MovieInfo> GetMovieInfo(string id, string provider, string language,
        CancellationToken cancellationToken)
    {
        return await GetMovieInfo(id, provider, true, language, cancellationToken);
    }

    public static async Task<MovieInfo> GetMovieInfo(string id, string provider, bool lazy, string language,
        CancellationToken cancellationToken)
    {
        var url = ComposeInfoApiUrl(MovieInfoApi, id, provider, language, lazy);
        return await GetDataFromApi<MovieInfo>(url, cancellationToken);
    }

    public static async Task<List<ActorSearchResult>> SearchActor(string keyword, string provider,
        CancellationToken cancellationToken)
    {
        return await SearchActor(keyword, provider, false, cancellationToken);
    }

    public static async Task<List<ActorSearchResult>> SearchActor(string keyword, string provider, bool lazy,
        CancellationToken cancellationToken)
    {
        var url = ComposeSearchApiUrl(ActorSearchApi, keyword, provider, lazy);
        return await GetDataFromApi<List<ActorSearchResult>>(url, cancellationToken);
    }

    public static async Task<List<MovieSearchResult>> SearchMovie(string keyword, string provider,
        CancellationToken cancellationToken)
    {
        return await SearchMovie(keyword, provider, false, cancellationToken);
    }

    public static async Task<List<MovieSearchResult>> SearchMovie(string keyword, string provider, bool lazy,
        CancellationToken cancellationToken)
    {
        var url = ComposeSearchApiUrl(MovieSearchApi, keyword, provider, lazy);
        return await GetDataFromApi<List<MovieSearchResult>>(url, cancellationToken);
    }

    private static async Task<T> GetDataFromApi<T>(string url, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var httpClient = new HttpClient();
        // Set default timeout: 2 minutes.
        httpClient.Timeout = TimeSpan.FromSeconds(120);
        // Set Accept JSON header.
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        // Set User-Agent header.
        httpClient.DefaultRequestHeaders.Add("User-Agent", Constant.UserAgent);
        // Set Authorization API Token.
        if (!string.IsNullOrWhiteSpace(Config.Token))
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", Config.Token);

        var response = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStreamAsync(cancellationToken));
    }
}