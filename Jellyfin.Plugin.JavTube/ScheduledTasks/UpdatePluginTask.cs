#if __EMBY__
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Jellyfin.Plugin.JavTube.Extensions;
using MediaBrowser.Common;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.IO;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Tasks;
using HttpRequestOptions = MediaBrowser.Common.Net.HttpRequestOptions;

namespace Jellyfin.Plugin.JavTube.ScheduledTasks;

public class UpdatePluginTask : IScheduledTask
{
    private readonly IApplicationHost _applicationHost;
    private readonly IApplicationPaths _applicationPaths;
    private readonly IHttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly IZipClient _zipClient;

    public UpdatePluginTask(IApplicationHost applicationHost, IApplicationPaths applicationPaths,
        IHttpClient httpClient, ILogManager logManager,
        IZipClient zipClient)
    {
        _applicationHost = applicationHost;
        _applicationPaths = applicationPaths;
        _httpClient = httpClient;
        _logger = logManager.CreateLogger<UpdatePluginTask>();
        _zipClient = zipClient;
    }

    private static string CurrentVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString();

    public string Key => $"{Plugin.Instance.Name}UpdatePlugin";

    public string Name => "Update Plugin";

    public string Description => $"Update {Plugin.Instance.Name} plugin to latest version.";

    public string Category => Plugin.Instance.Name;

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        yield return new TaskTriggerInfo
        {
            Type = TaskTriggerInfo.TriggerDaily,
            TimeOfDayTicks = TimeSpan.FromHours(5).Ticks
        };
    }

    public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
    {
        await Task.Yield();
        progress?.Report(0);

        try
        {
            var response = await _httpClient.GetResponse(new HttpRequestOptions
            {
                Url = "https://api.github.com/repos/javtube/jellyfin-plugin-javtube/releases/latest",
                CancellationToken = cancellationToken,
                EnableDefaultUserAgent = true
            });

            if (response.StatusCode != HttpStatusCode.OK)
                throw new Exception($"api status code: {response.StatusCode}");

            var apiResult = JsonSerializer.Deserialize<GithubApiResult>(response.Content);

            var currentVersion = ParseVersion(CurrentVersion);
            var remoteVersion = ParseVersion(apiResult?.TagName);

            if (currentVersion.CompareTo(remoteVersion) < 0)
            {
                _logger.Info("New plugin version found: {0}", remoteVersion);

                var url = apiResult?.Assets
                    .Where(asset => asset.Name.StartsWith("Emby") && asset.Name.EndsWith(".zip")).ToArray()
                    .FirstOrDefault()
                    ?.BrowserDownloadUrl;
                if (string.IsNullOrEmpty(url))
                    throw new Exception("download url is empty");

                var zipResp = await _httpClient.GetResponse(new HttpRequestOptions
                {
                    Url = url,
                    CancellationToken = cancellationToken,
                    EnableDefaultUserAgent = true,
                    Progress = progress
                });

                _zipClient.ExtractAllFromZip(zipResp.Content, _applicationPaths.PluginsPath, true);

                _logger.Info("Plugin update is complete");

                _applicationHost.NotifyPendingRestart();
            }
            else
            {
                _logger.Info("No need to update");
            }
        }
        catch (Exception e)
        {
            _logger.Error("Update error: {0}", e.Message);
        }

        progress?.Report(100);
    }

    private static Version ParseVersion(string v)
    {
        return new Version(v.StartsWith("v") ? v[1..] : v);
    }

    public class GithubApiResult
    {
        [JsonPropertyName("tag_name")] public string TagName { get; set; }
        [JsonPropertyName("assets")] public GithubApiAsset[] Assets { get; set; }
    }

    public class GithubApiAsset
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string BrowserDownloadUrl { get; set; }
    }
}

#endif