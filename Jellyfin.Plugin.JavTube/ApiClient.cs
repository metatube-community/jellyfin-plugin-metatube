using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Web;
using Jellyfin.Plugin.JavTube.Models;

namespace Jellyfin.Plugin.JavTube;

public static class ApiClient
{
    private const string ActorInfoApi = "/api/actor";
    private const string MovieInfoApi = "/api/movie";
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

    private static string ComposeImageApiUrl(string path, string id, string provider, string url, double position,
        bool auto)
    {
        return ComposeUrl(path, new NameValueCollection
        {
            { "id", id },
            { "provider", provider },
            { "url", url },
            { "pos", position.ToString("R") },
            { "auto", auto.ToString() }
        });
    }

    private static string ComposeInfoApiUrl(string path, string id, string provider, string url, bool lazy)
    {
        return ComposeUrl(path, new NameValueCollection
        {
            { "id", id },
            { "provider", provider },
            { "url", url },
            { "lazy", lazy.ToString() }
        });
    }

    private static string ComposeSearchApiUrl(string path, string keyword, string provider, bool lazy)
    {
        return ComposeUrl(path, new NameValueCollection
        {
            { "keyword", keyword },
            { "provider", provider },
            { "lazy", lazy.ToString() }
        });
    }

    public static string GetPrimaryImageApiUrl(string id, string provider, double position = -1)
    {
        return ComposeImageApiUrl(PrimaryImageApi, id, provider, string.Empty, position, false);
    }

    public static string GetPrimaryImageApiUrl(string id, string provider, string url, double position = -1,
        bool auto = false)
    {
        return ComposeImageApiUrl(PrimaryImageApi, id, provider, url, position, auto);
    }

    public static string GetThumbImageApiUrl(string id, string provider)
    {
        return ComposeImageApiUrl(ThumbImageApi, id, provider, string.Empty, -1, false);
    }

    public static string GetThumbImageApiUrl(string id, string provider, string url, double position = -1,
        bool auto = false)
    {
        return ComposeImageApiUrl(ThumbImageApi, id, provider, url, position, auto);
    }

    public static string GetBackdropImageApiUrl(string id, string provider)
    {
        return ComposeImageApiUrl(BackdropImageApi, id, provider, string.Empty, -1, false);
    }

    public static string GetBackdropImageApiUrl(string id, string provider, string url, double position = -1,
        bool auto = false)
    {
        return ComposeImageApiUrl(BackdropImageApi, id, provider, url, position, auto);
    }

    public static async Task<ActorInfoModel> GetActorInfo(string id, string provider,
        CancellationToken cancellationToken)
    {
        return await GetActorInfo(id, provider, string.Empty, true, cancellationToken);
    }

    public static async Task<ActorInfoModel> GetActorInfo(string id, string provider, bool lazy,
        CancellationToken cancellationToken)
    {
        return await GetActorInfo(id, provider, string.Empty, lazy, cancellationToken);
    }

    public static async Task<ActorInfoModel> GetActorInfo(string id, string provider, string url, bool lazy,
        CancellationToken cancellationToken)
    {
        var apiUrl = ComposeInfoApiUrl(ActorInfoApi, id, provider, url, lazy);
        return await GetDataFromApi<ActorInfoModel>(apiUrl, cancellationToken);
    }

    public static async Task<MovieInfoModel> GetMovieInfo(string id, string provider,
        CancellationToken cancellationToken)
    {
        return await GetMovieInfo(id, provider, string.Empty, true, cancellationToken);
    }

    public static async Task<MovieInfoModel> GetMovieInfo(string id, string provider, bool lazy,
        CancellationToken cancellationToken)
    {
        return await GetMovieInfo(id, provider, string.Empty, lazy, cancellationToken);
    }

    public static async Task<MovieInfoModel> GetMovieInfo(string id, string provider, string url, bool lazy,
        CancellationToken cancellationToken)
    {
        var apiUrl = ComposeInfoApiUrl(MovieInfoApi, id, provider, url, lazy);
        return await GetDataFromApi<MovieInfoModel>(apiUrl, cancellationToken);
    }

    public static async Task<List<ActorSearchResultModel>> SearchActor(string keyword,
        CancellationToken cancellationToken)
    {
        return await SearchActor(keyword, string.Empty, false, cancellationToken);
    }

    public static async Task<List<ActorSearchResultModel>> SearchActor(string keyword, string provider,
        CancellationToken cancellationToken)
    {
        return await SearchActor(keyword, provider, false, cancellationToken);
    }

    public static async Task<List<ActorSearchResultModel>> SearchActor(string keyword, string provider,
        bool lazy, CancellationToken cancellationToken)
    {
        var apiUrl = ComposeSearchApiUrl(ActorSearchApi, keyword, provider, lazy);
        return await GetDataFromApi<List<ActorSearchResultModel>>(apiUrl, cancellationToken);
    }

    public static async Task<List<MovieSearchResultModel>> SearchMovie(string keyword,
        CancellationToken cancellationToken)
    {
        return await SearchMovie(keyword, string.Empty, false, cancellationToken);
    }

    public static async Task<List<MovieSearchResultModel>> SearchMovie(string keyword, string provider,
        CancellationToken cancellationToken)
    {
        return await SearchMovie(keyword, provider, false, cancellationToken);
    }

    public static async Task<List<MovieSearchResultModel>> SearchMovie(string keyword, string provider,
        bool lazy, CancellationToken cancellationToken)
    {
        var apiUrl = ComposeSearchApiUrl(MovieSearchApi, keyword, provider, lazy);
        return await GetDataFromApi<List<MovieSearchResultModel>>(apiUrl, cancellationToken);
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