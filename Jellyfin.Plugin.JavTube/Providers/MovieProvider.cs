#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;
#else
using Microsoft.Extensions.Logging;
#endif
using Jellyfin.Plugin.JavTube.Extensions;
using Jellyfin.Plugin.JavTube.Models;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.JavTube.Providers;

public class MovieProvider : BaseProvider, IRemoteMetadataProvider<Movie, MovieInfo>, IHasOrder
{
#if __EMBY__
    public MovieProvider(IHttpClient httpClient, ILogManager logManager) : base(
        httpClient, logManager.CreateLogger<MovieProvider>())
#else
    public MovieProvider(IHttpClientFactory httpClientFactory, ILogger<MovieProvider> logger) : base(
        httpClientFactory, logger)
#endif
    {
        // Init
    }

    public int Order => 1;

    public string Name => Constant.JavTube;

    public async Task<MetadataResult<Movie>> GetMetadata(MovieInfo info,
        CancellationToken cancellationToken)
    {
        var pid = info.GetProviderIdModel(Constant.JavTube);
        if (string.IsNullOrWhiteSpace(pid.Id) || string.IsNullOrWhiteSpace(pid.Provider))
        {
            // Search movie and pick the first result.
            var searchResults = (await GetSearchResults(info, cancellationToken)).ToList();
            if (searchResults.Any())
            {
                var firstResult = searchResults.First();
                pid = firstResult.GetProviderIdModel(Constant.JavTube);
            }
        }

        var m = Plugin.Instance.Configuration.EnableAutoTranslate
            ? await ApiClient.GetMovieInfo(pid.Id, pid.Provider, info.MetadataLanguage, cancellationToken)
            : await ApiClient.GetMovieInfo(pid.Id, pid.Provider, cancellationToken);

        var result = new MetadataResult<Movie>
        {
            Item = new Movie
            {
                Name = FormatName(m),
                OriginalTitle = m.Title,
                Overview = m.Summary,
                Tagline = m.Series,
                Genres = m.Tags,
                PremiereDate = m.ReleaseDate.ValidDateTime(),
                ProductionYear = m.ReleaseDate.ValidDateTime()?.Year,
                OfficialRating = Constant.Rating
            },
            HasMetadata = true
        };
        result.Item.SetProviderIdModel(Constant.JavTube, m);

        // Set official score.
        result.Item.CommunityRating = m.Score > 0 ? m.Score * 2 : null;

        // Set studios: maker > publisher.
        result.Item.Studios = !string.IsNullOrWhiteSpace(m.Maker)
            ? new[] { m.Maker }
            : !string.IsNullOrWhiteSpace(m.Publisher)
                ? new[] { m.Publisher }
                : null;

        // Add director.
        if (!string.IsNullOrWhiteSpace(m.Director))
            result.AddPerson(new PersonInfo
            {
                Name = m.Director,
                Type = PersonType.Director
            });

        // Add actors.
        foreach (var name in m.Actors)
            result.AddPerson(new PersonInfo
            {
                Name = name,
                Type = PersonType.Actor,
                ImageUrl = await GetActorImageUrl(name, cancellationToken)
            });

        return result;
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(MovieInfo info,
        CancellationToken cancellationToken)
    {
        var pid = info.GetProviderIdModel(Constant.JavTube);
        if (string.IsNullOrWhiteSpace(pid.Id))
        {
            // Search movie by name.
            pid.Id = info.Name;
        }

        LogInfo("Search for movie: {0}", pid.Id);

        var results = new List<RemoteSearchResult>();

        var searchResults = await ApiClient.SearchMovie(pid.Id, pid.Provider, cancellationToken);
        if (!searchResults.Any())
        {
            LogInfo("Movie not found: {0}", pid.Id);
            return results;
        }

        foreach (var m in searchResults)
        {
            var result = new RemoteSearchResult
            {
                Name = FormatName(m),
                SearchProviderName = Name,
                PremiereDate = m.ReleaseDate.ValidDateTime(),
                ProductionYear = m.ReleaseDate.ValidDateTime()?.Year,
                ImageUrl = ApiClient.GetPrimaryImageApiUrl(m.Id, m.Provider, m.ThumbUrl, 0.5)
            };
            result.SetProviderIdModel(Constant.JavTube, m);
            results.Add(result);
        }

        return results;
    }

    private static string FormatName(MovieSearchResultModel m)
    {
        return $"{m.Number} {m.Title}";
    }

    private async Task<string> GetActorImageUrl(string name, CancellationToken cancellationToken)
    {
        try
        {
            // Use GFriends as actor image provider.
            const string gFriends = "GFriends";
            return (await ApiClient.GetActorInfo(name, gFriends, cancellationToken)).Images[0];
        }
        catch (Exception e)
        {
            LogError("Get Actor Image Error: {0}: {1}", name, e.Message);
            return string.Empty;
        }
    }
}