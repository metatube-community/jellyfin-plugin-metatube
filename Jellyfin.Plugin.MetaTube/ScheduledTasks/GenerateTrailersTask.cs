using System.Text;
using Jellyfin.Plugin.MetaTube.Extensions;
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

namespace Jellyfin.Plugin.MetaTube.ScheduledTasks;

public class GenerateTrailersTask : IScheduledTask
{
    // Emby: trailers can be stored in a trailers sub-folder.
    // https://support.emby.media/support/solutions/articles/44001159193-trailers
    private const string TrailersFolder = "trailers";

    // Uniform suffix for all trailer files.
    private const string TrailerFileSuffix = "-Trailer.strm";
    private const string TrailerSearchPattern = $"*{TrailerFileSuffix}";

    // UTF-8 without BOM encoding.
    private static readonly Encoding Utf8WithoutBom = new UTF8Encoding(false);

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

    public string Description => $"Generates video trailers provided by {Plugin.Instance.Name} in library.";

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
        // Stop the task if disabled.
        if (!Plugin.Instance.Configuration.EnableTrailers)
            return;

        await Task.Yield();

        progress?.Report(0);

        var result = _libraryManager.QueryItems(new InternalItemsQuery
        {
            MediaTypes = new[] { MediaType.Video },
#if __EMBY__
            HasAnyProviderId = new[] { Plugin.Instance.Name },
            IncludeItemTypes = new[] { nameof(Movie) },
#else
            HasAnyProviderId = new Dictionary<string, string> { { Plugin.Instance.Name, string.Empty } },
            IncludeItemTypes = new[] { BaseItemKind.Movie }
#endif
        });
        var items = result.Items.ToList();

        foreach (var (idx, item) in items.WithIndex())
        {
            cancellationToken.ThrowIfCancellationRequested();
            progress?.Report((double)idx / items.Count * 100);

            try
            {
                var trailersFolderPath = Path.Join(item.ContainingFolderPath, TrailersFolder);

                // Skip if contains .ignore file.
                if (File.Exists(Path.Join(trailersFolderPath, ".ignore")))
                    continue;

                var trailerUrl = item.GetTrailerUrl();

                // Skip if no remote trailers.
                if (string.IsNullOrWhiteSpace(trailerUrl))
                {
                    if (Directory.Exists(trailersFolderPath))
                    {
                        // Delete obsolete trailer files.
                        DeleteFiles(trailersFolderPath, TrailerSearchPattern);

                        // Delete directory if empty.
                        DeleteDirectoryIfEmpty(trailersFolderPath);
                    }

                    continue;
                }

                var trailerFilePath = Path.Join(trailersFolderPath,
                    $"{item.Name.Split().First()}{TrailerFileSuffix}");

#if __EMBY__
                var lastSavedUtcDateTime = item.DateLastSaved.UtcDateTime;
#else
                var lastSavedUtcDateTime = item.DateLastSaved.ToUniversalTime();
#endif

                // When trailer file already exists.
                if (File.Exists(trailerFilePath))
                {
                    // Skip if trailer file is up to date.
                    if (File.GetLastWriteTimeUtc(trailerFilePath).CompareTo(lastSavedUtcDateTime) >= 0)
                        continue;

                    // Skip if content is not modified.
                    if (string.Equals(await File.ReadAllTextAsync(trailerFilePath, cancellationToken), trailerUrl))
                    {
                        File.SetLastWriteTimeUtc(trailerFilePath, DateTime.UtcNow);
                        continue;
                    }
                }

                // Create trailers folder if not exists.
                if (!Directory.Exists(trailersFolderPath))
                    Directory.CreateDirectory(trailersFolderPath);

                // Delete other trailer files, if any.
                DeleteFiles(trailersFolderPath, TrailerSearchPattern, trailerFilePath);

                _logger.Info("Generate trailer for video {0} at {1}", item.Name, trailerFilePath);

                // Write .strm trailer file.
                await File.WriteAllTextAsync(trailerFilePath, trailerUrl, Utf8WithoutBom, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.Error("Generate trailer for video {0} error: {1}", item.Name, e.Message);
            }
        }

        progress?.Report(100);
    }

    private static void DeleteFiles(string path, string searchPattern, params string[] excludedFiles)
    {
        DeleteFiles(Directory.GetFiles(path, searchPattern).Where(file => !excludedFiles.Contains(file)));
    }

    private static void DeleteFiles(IEnumerable<string> files)
    {
        foreach (var file in files) File.Delete(file);
    }

    private static void DeleteDirectoryIfEmpty(string path)
    {
        if (!Directory.GetDirectories(path).Any() && !Directory.GetFiles(path).Any())
            Directory.Delete(path);
    }
}
