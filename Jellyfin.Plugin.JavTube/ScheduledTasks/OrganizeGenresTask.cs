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
    {
        await Task.Yield();
#else
    public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        Task.Yield();
#endif
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

            var genres = item.Genres?.ToList() ?? new List<string>();

            // Replace Genres.
            foreach (var genre in genres.Where(genre => GenreHelper.SubstitutionTable.ContainsKey(genre)).ToArray())
            {
                var value = GenreHelper.SubstitutionTable[genre];
                if (string.IsNullOrEmpty(value))
                    genres.Remove(genre); // should just be removed.
                else
                    genres[genres.IndexOf(genre)] = value; // replace.
            }

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
            var orderedGenres = genres.Distinct().OrderByString(g => g).ToList();

            // Skip updating item if equal.
            if (!orderedGenres.Any() ||
                (item.Genres?.SequenceEqual(orderedGenres, StringComparer.Ordinal)).GetValueOrDefault(false))
                continue;

            item.Genres = orderedGenres.ToArray();

            _logger.Info("OrganizeGenres for video: {0}", item.Name);

#if __EMBY__
            _libraryManager.UpdateItem(item, item, ItemUpdateType.MetadataEdit, null);
#else
            _libraryManager
                .UpdateItemAsync(item, item, ItemUpdateType.MetadataEdit, cancellationToken)
                .ConfigureAwait(false);
#endif
        }

        progress?.Report(100);

#if !__EMBY__
        return Task.CompletedTask;
#endif
    }
}