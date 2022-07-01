using Jellyfin.Plugin.JavTube.Extensions;
using Jellyfin.Plugin.JavTube.Metadata;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Providers;
#if __EMBY__
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Logging;

#else
using Microsoft.Extensions.Logging;
#endif

namespace Jellyfin.Plugin.JavTube.Providers;

public class ActorProvider : BaseProvider, IRemoteMetadataProvider<Person, PersonLookupInfo>, IHasOrder
{
#if __EMBY__
    public ActorProvider(IHttpClient httpClient, ILogManager logManager) : base(
        httpClient,
        logManager.CreateLogger<ActorProvider>())
#else
    public ActorProvider(IHttpClientFactory httpClientFactory, ILogger<ActorProvider> logger) : base(
        httpClientFactory, logger)
#endif
    {
    }

    public async Task<MetadataResult<Person>> GetMetadata(PersonLookupInfo info,
        CancellationToken cancellationToken)
    {
        var pid = info.GetPid(Name);
        if (string.IsNullOrWhiteSpace(pid.Id) || string.IsNullOrWhiteSpace(pid.Provider))
        {
            var firstResult = (await GetSearchResults(info, cancellationToken)).FirstOrDefault();
            if (firstResult != null) pid = firstResult.GetPid(Name);
        }

        Logger.Info("Get actor info: {0}", pid.ToString());

        var m = await ApiClient.GetActorInfoAsync(pid.Provider, pid.Id, cancellationToken);

        var result = new MetadataResult<Person>
        {
            Item = new Person
            {
                Name = m.Name,
                PremiereDate = m.Birthday.GetValidDateTime(),
                ProductionYear = m.Birthday.GetValidYear(),
                Overview = FormatOverview(m)
            },
            HasMetadata = true
        };

        // Set ProviderIdModel.
        result.Item.SetPid(Name, m.Provider, m.Id);

        // Set actor nationality.
        if (!string.IsNullOrWhiteSpace(m.Nationality))
            result.Item.ProductionLocations = new[] { m.Nationality };

        return result;
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
        PersonLookupInfo info, CancellationToken cancellationToken)
    {
        var pid = info.GetPid(Name);

        var searchResults = new List<ActorSearchResult>();
        if (string.IsNullOrWhiteSpace(pid.Id))
        {
            // Search actor by name.
            Logger.Info("Search for actor: {0}", info.Name);
            searchResults.AddRange(await ApiClient.SearchActorAsync(info.Name, pid.Provider, cancellationToken));
        }
        else
        {
            // Exact search.
            Logger.Info("Search for actor: {0}", pid.ToString());
            searchResults.Add(await ApiClient.GetActorInfoAsync(pid.Provider, pid.Id,
                pid.Update != true, cancellationToken));
        }

        var results = new List<RemoteSearchResult>();
        if (!searchResults.Any())
        {
            Logger.Warn("Actor not found: {0}", pid.Id);
            return results;
        }

        foreach (var m in searchResults)
        {
            var result = new RemoteSearchResult
            {
                Name = m.Name,
                SearchProviderName = Name,
                ImageUrl = m.Images.Length > 0
                    ? ApiClient.GetPrimaryImageApiUrl(m.Provider, m.Id, m.Images[0], 0.5, true)
                    : string.Empty
            };
            result.SetPid(Name, m.Provider, m.Id);
            results.Add(result);
        }

        return results;
    }

    private static string FormatOverview(ActorInfo a)
    {
        string G(string k, string v)
        {
            return !string.IsNullOrWhiteSpace(v) ? $"{k}: {v}\n" : string.Empty;
        }

        var overview = string.Empty;
        overview += G("身長", a.Height > 0 ? $"{a.Height}cm" : null);
        overview += G("血液型", a.BloodType);
        overview += G("ブラのサイズ", a.CupSize);
        overview += G("スリーサイズ", a.Measurements);
        overview += G("趣味", a.Hobby);
        overview += G("特技", a.Skill);
        return overview;
    }
}