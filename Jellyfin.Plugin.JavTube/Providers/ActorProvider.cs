using Jellyfin.Plugin.JavTube.Extensions;
using Jellyfin.Plugin.JavTube.Models;
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
        // Nothing
    }

    public int Order => 1;

    public string Name => Constant.JavTube;

    public async Task<MetadataResult<Person>> GetMetadata(PersonLookupInfo info,
        CancellationToken cancellationToken)
    {
        var pm = info.GetProviderIdModel(Name);
        if (string.IsNullOrWhiteSpace(pm.Id) || string.IsNullOrWhiteSpace(pm.Provider))
        {
            var searchResults = (await GetSearchResults(info, cancellationToken)
                .ConfigureAwait(false)).ToList();
            if (searchResults.Any())
            {
                var firstResult = searchResults.First();
                pm = firstResult.GetProviderIdModel(Name);
            }
        }

        var m = await ApiClient.GetActorInfo(pm.Id, pm.Provider, cancellationToken);

        var result = new MetadataResult<Person>
        {
            Item = new Person
            {
                Name = m.Name,
                PremiereDate = m.Birthday.ValidDateTime(),
                ProductionYear = m.Birthday.ValidDateTime()?.Year,
                Overview = FormatOverview(m)
            },
            HasMetadata = true
        };

        if (!string.IsNullOrWhiteSpace(m.Nationality))
            result.Item.ProductionLocations = new[] { m.Nationality };

        result.Item.SetProviderIdModel(Name, m);

        return result;
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(
        PersonLookupInfo info, CancellationToken cancellationToken)
    {
        var pm = info.GetProviderIdModel(Name);
        if (string.IsNullOrWhiteSpace(pm.Id))
            pm.Id = info.Name;

        var searchResults = await ApiClient.SearchActor(pm.Id, pm.Provider, cancellationToken);

        var results = new List<RemoteSearchResult>();
        foreach (var m in searchResults)
        {
            var result = new RemoteSearchResult
            {
                Name = m.Name,
                SearchProviderName = Name
            };
            if (m.Images.Length > 0)
                result.ImageUrl = ApiClient.GetPrimaryImageApiUrl(m.Id, m.Provider, m.Images[0], auto: true);
            result.SetProviderIdModel(Name, m);
            results.Add(result);
        }

        return results;
    }

    private static string FormatOverview(ActorInfoModel a)
    {
        string G(string k, string v)
        {
            return !string.IsNullOrWhiteSpace(v) ? $"{k}: {v}\n" : string.Empty;
        }

        var overview = string.Empty;
        overview += G("身高", $"{a.Height}cm");
        overview += G("血型", $"{a.BloodType}");
        overview += G("罩杯", $"{a.CupSize}");
        overview += G("三围", $"{a.Measurements}");
        return overview;
    }
}