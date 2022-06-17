using System.Text;
using Jellyfin.Plugin.JavTube.Extensions;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Tasks;
#if __EMBY__
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Model.Logging;
#else
using Microsoft.Extensions.Logging;
using Jellyfin.Data.Enums;
#endif

namespace Jellyfin.Plugin.JavTube.ScheduledTasks;

public class GenerateTrailersTask : IScheduledTask
{
    // Emby: trailers can also be stored in a trailers sub-folder.
    // https://support.emby.media/support/solutions/articles/44001159193-trailers
    private const string TrailersFolder = "trailers";

    private readonly ILibraryManager _libraryManager;
    private readonly ILogger _logger;

#if __EMBY__
    public GenerateTrailersTask(ILogManager logManager, ILibraryManager libraryManager)
    {
        _logger = logManager.CreateLogger<GenerateTrailersTask>();
        _libraryManager = libraryManager;
    }
#else
    public GenerateTrailersTask(ILogger<GenerateTrailersTask> logger, ILibraryManager libraryManager)
    {
        _logger = logger;
        _libraryManager = libraryManager;
    }
#endif

    public string Key => $"{Plugin.Instance.Name}GenerateTrailers";

    public string Name => "Generate Trailers";

    public string Description => $"Generate video trailers provided by {Plugin.Instance.Name} in library.";

    public string Category => Plugin.Instance.Name;

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        yield return new TaskTriggerInfo
        {
            Type = TaskTriggerInfo.TriggerDaily,
            TimeOfDayTicks = TimeSpan.FromHours(1).Ticks
        };
    }

#if __EMBY__
    public async Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
#else
    public async Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
#endif
    {
        await Task.Yield();

        progress?.Report(0);

        var items = _libraryManager.GetItemList(new InternalItemsQuery
        {
            MediaTypes = new[] { MediaType.Video },
#if __EMBY__
            HasAnyProviderId = new[] { Plugin.Instance.Name },
            IncludeItemTypes = new[] { nameof(Movie) },
#else
            HasAnyProviderId = new Dictionary<string, string> { { Plugin.Instance.Name, string.Empty } },
            IncludeItemTypes = new[] { BaseItemKind.Movie }
#endif
        }).ToList();

        foreach (var (idx, item) in items.WithIndex())
        {
            progress?.Report((double)idx / items.Count * 100);

            var trailersFolder = Path.Join(item.ContainingFolderPath, TrailersFolder);

            // Skip if contains .ignore file.
            if (File.Exists(Path.Join(trailersFolder, ".ignore")))
                continue;

            // Skip if no remote trailers.
            if (item.RemoteTrailers?.Any() != true)
            {
                // Delete obsolete trailer files.
                TryDeleteFiles(trailersFolder, "*.strm");

                // Delete directory if empty.
                TryDeleteDirectory(trailersFolder);

                continue;
            }

            var trailerFile = Path.Join(trailersFolder,
                $"Trailer - {(!string.IsNullOrWhiteSpace(item.SortName) ? item.SortName : item.Name)}.strm");

            // Skip if trailer file already exists.
            if (File.Exists(trailerFile))
                continue;

            // Create trailers folder if not exists.
            if (!Directory.Exists(trailersFolder))
                Directory.CreateDirectory(trailersFolder);

            // Delete other trailer files.
            TryDeleteFiles(trailersFolder, "*.strm");

#if __EMBY__
            var trailerUrl = item.RemoteTrailers.First();
#else
            var trailerUrl = item.RemoteTrailers.First().Url;
#endif

            _logger.Info("Generate trailer for video: {0}", item.Name);

            // Write trailer .strm file.
            await File.WriteAllTextAsync(trailerFile, trailerUrl, Encoding.UTF8, cancellationToken);
        }

        progress?.Report(100);
    }

    private static void TryDeleteFiles(string path, string searchPattern)
    {
        try
        {
            TryDeleteFiles(Directory.GetFiles(path, searchPattern));
        }
        catch (Exception)
        {
            // ignored.
        }
    }

    private static void TryDeleteFiles(IEnumerable<string> files)
    {
        foreach (var file in files)
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception)
            {
                // ignored.
            }
        }
    }

    private static void TryDeleteDirectory(string path)
    {
        try
        {
            Directory.Delete(path);
        }
        catch (Exception)
        {
            // ignored.
        }
    }
}