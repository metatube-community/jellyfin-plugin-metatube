using Jellyfin.Plugin.JavTube.Extensions;
using Jellyfin.Plugin.JavTube.Models;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;

#else
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.JavTube.Providers;

public class MovieProvider : BaseProvider, IRemoteMetadataProvider<Movie, MovieInfo>, IHasOrder
{
    private const string GFriends = "GFriends";
    private const string Rating = "JP-18+";

#if __EMBY__
    public MovieProvider(IHttpClient httpClient, ILogManager logManager) : base(
        httpClient, logManager.CreateLogger<MovieProvider>())
#else
    public MovieProvider(IHttpClientFactory httpClientFactory, ILogger<MovieProvider> logger) : base(
        httpClientFactory, logger)
#endif
    {
    }

    public async Task<MetadataResult<Movie>> GetMetadata(MovieInfo info,
        CancellationToken cancellationToken)
    {
        var pid = info.GetProviderIdModel(Name);
        if (string.IsNullOrWhiteSpace(pid.Id) || string.IsNullOrWhiteSpace(pid.Provider))
        {
            // Search movie and pick the first result.
            var searchResults = (await GetSearchResults(info, cancellationToken)).ToList();
            if (searchResults.Any())
            {
                var firstResult = searchResults.First();
                pid = firstResult.GetProviderIdModel(Name);
            }
        }

        Logger.Info("Get movie info: {0}", pid.Serialize());

        // var m = Plugin.Instance.Configuration.EnableAutoTranslation
        //     ? await ApiClient.GetMovieInfo(pid.Id, pid.Provider, info.MetadataLanguage, cancellationToken)
        //     : await ApiClient.GetMovieInfo(pid.Id, pid.Provider, cancellationToken);

        var m = await ApiClient.GetMovieInfo(pid.Provider, pid.Id, cancellationToken);

        var result = new MetadataResult<Movie>
        {
            Item = new Movie
            {
                Name = $"{m.Number} {m.Title}",
                OriginalTitle = m.Title,
                Overview = m.Summary,
                Tagline = m.Series,
                Genres = m.Tags,
                OfficialRating = Rating,
                PremiereDate = m.ReleaseDate.TryGetValidDateTime(),
                ProductionYear = m.ReleaseDate.TryGetValidDateTime()?.Year,
                CommunityRating = m.Score > 0 ? m.Score * 2 : null
            },
            HasMetadata = true
        };

        // Set ProviderIdModel.
        result.Item.SetProviderIdModel(Name, new ProviderIdModel
        {
            Provider = m.Provider,
            Id = m.Id,
            Position = pid.Position
        });

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
        var pid = info.GetProviderIdModel(Name);

        var searchResults = new List<MovieSearchResultModel>();
        if (string.IsNullOrWhiteSpace(pid.Id) || string.IsNullOrWhiteSpace(pid.Provider))
        {
            // Search movie by name.
            Logger.Info("Search for movie: {0}", info.Name);
            searchResults.AddRange(await ApiClient.SearchMovie(info.Name, pid.Provider, cancellationToken));
        }
        else
        {
            // Exact search.
            Logger.Info("Search for movie: {0}", pid.Serialize());
            searchResults.Add(await ApiClient.GetMovieInfo(pid.Provider, pid.Id,
                pid.UpdateInfo != true, cancellationToken));
        }

        var results = new List<RemoteSearchResult>();
        if (!searchResults.Any())
        {
            Logger.Warn("Movie not found: {0}", pid.Id);
            return results;
        }

        foreach (var m in searchResults)
        {
            var result = new RemoteSearchResult
            {
                Name = $"[{m.Provider}] {m.Number} {m.Title}",
                SearchProviderName = Name,
                PremiereDate = m.ReleaseDate.TryGetValidDateTime(),
                ProductionYear = m.ReleaseDate.TryGetValidYear(),
                ImageUrl = ApiClient.GetPrimaryImageApiUrl(m.Provider, m.Id, m.ThumbUrl, 1.0, true)
            };
            result.SetProviderIdModel(Name, new ProviderIdModel
            {
                Provider = m.Provider,
                Id = m.Id,
                Position = pid.Position
            });
            results.Add(result);
        }

        return results;
    }

    private async Task<string> GetActorImageUrl(string name, CancellationToken cancellationToken)
    {
        try
        {
            // Use GFriends as actor image provider.
            return (await ApiClient.GetActorInfo(GFriends, name, cancellationToken)).Images[0];
        }
        catch (Exception e)
        {
            Logger.Error("Get actor image error: {0} ({1})", name, e.Message);
            return string.Empty;
        }
    }
}