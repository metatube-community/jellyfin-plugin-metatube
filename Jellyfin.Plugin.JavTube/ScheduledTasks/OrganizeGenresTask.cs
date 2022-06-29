using Jellyfin.Plugin.JavTube.Extensions;
using Jellyfin.Plugin.JavTube.Helpers;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Sorting;
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

public class OrganizeGenresTask : IScheduledTask
{
    private readonly ILibraryManager _libraryManager;
    private readonly ILogger _logger;

#if __EMBY__
    public OrganizeGenresTask(ILogManager logManager, ILibraryManager libraryManager)
    {
        _logger = logManager.CreateLogger<OrganizeGenresTask>();
        _libraryManager = libraryManager;
    }
#else
    public OrganizeGenresTask(ILogger<OrganizeGenresTask> logger, ILibraryManager libraryManager)
    {
        _logger = logger;
        _libraryManager = libraryManager;
    }
#endif

    public string Key => $"{Plugin.Instance.Name}OrganizeGenres";

    public string Name => "Organize Genres";

    public string Description => $"Organize metadata genres provided by {Plugin.Instance.Name} in library.";

    public string Category => Plugin.Instance.Name;

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        yield return new TaskTriggerInfo
        {
            Type = TaskTriggerInfo.TriggerDaily,
            TimeOfDayTicks = TimeSpan.FromHours(3).Ticks
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

            var genres = Plugin.Instance.Configuration.EnableGenreSubstitution
                ? item.Genres.Substitute(Plugin.Instance.Configuration.GetGenreSubstitutionTable()).ToList()
                : item.Genres?.ToList() ?? new List<string>();

            try
            {
                // Add or Remove `ChineseSubtitle` Genre.
                if (GenreHelper.HasEmbeddedChineseSubtitle(item.FileNameWithoutExtension) ||
                    GenreHelper.HasExternalChineseSubtitle(item.Path))
                    genres.Add(GenreHelper.ChineseSubtitle);
                else
                    genres.RemoveAll(s => s.Equals(GenreHelper.ChineseSubtitle));
            }
            catch (Exception e)
            {
                _logger.Error("Chinese subtitle for video {0}: {1}", item.Name, e.Message);
            }

            // Remove Duplicates.
            var orderedGenres = genres.Distinct().OrderByString(genre => genre).ToList();

            // Skip updating item if equal.
            if (!orderedGenres.Any() ||
                (item.Genres?.SequenceEqual(orderedGenres, StringComparer.OrdinalIgnoreCase)).GetValueOrDefault(false))
                continue;

            item.Genres = orderedGenres.ToArray();

            _logger.Info("Organize genres for video: {0}", item.Name);

#if __EMBY__
            _libraryManager.UpdateItem(item, item, ItemUpdateType.MetadataEdit, null);
#else
            await _libraryManager
                .UpdateItemAsync(item, item, ItemUpdateType.MetadataEdit, cancellationToken)
                .ConfigureAwait(false);
#endif
        }

        progress?.Report(100);
    }
}