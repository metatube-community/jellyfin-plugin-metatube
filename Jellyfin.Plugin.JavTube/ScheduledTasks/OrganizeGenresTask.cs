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

    public string Key => $"{Constant.JavTube}OrganizeGenres";

    public string Name => "Organize Genres";

    public string Description => $"Organize metadata genres provided by {Constant.JavTube} in library.";

    public string Category => Constant.JavTube;

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
            HasAnyProviderId = new[] { Constant.JavTube },
            IncludeItemTypes = new[] { nameof(Movie) },
#else
            HasAnyProviderId = new Dictionary<string, string> { { Constant.JavTube, string.Empty } },
            IncludeItemTypes = new[] { BaseItemKind.Movie }
#endif
        }).ToList();

        foreach (var (idx, item) in items.WithIndex())
        {
            progress?.Report((double)idx / items.Count * 100);

            var genres = item.Genres?.ToList() ?? new List<string>();

            // Replace Genres
            foreach (var genre in genres.Where(genre => Genres.Substitution.ContainsKey(genre)).ToArray())
            {
                var value = Genres.Substitution[genre];
                if (string.IsNullOrEmpty(value))
                    genres.Remove(genre); // should just be removed
                else
                    genres[genres.IndexOf(genre)] = value; // replace
            }

            try
            {
                // Add or Remove `ChineseSubtitle` Genre
                if (Genres.HasChineseSubtitle(item.FileNameWithoutExtension) ||
                    Genres.HasExternalChineseSubtitle(item.Path))
                    genres.Add(Genres.ChineseSubtitle);
                else
                    genres.RemoveAll(s => s.Equals(Genres.ChineseSubtitle));
            }
            catch (Exception e)
            {
#if __EMBY__
                _logger.Error("Chinese subtitle for video {0}: {1}", item.Name, e.Message);
#else
                _logger.LogError("Chinese subtitle for video {Name}: {Message}", item.Name, e.Message);
#endif
            }

            // Remove Duplicates
            var orderedGenres = genres.Distinct().OrderByString(g => g).ToList();

            // Skip updating item if equal
            if (!orderedGenres.Any() ||
                (item.Genres?.SequenceEqual(orderedGenres, StringComparer.Ordinal)).GetValueOrDefault(false))
                continue;

            item.Genres = orderedGenres.ToArray();

#if __EMBY__
            _logger.Info("OrganizeGenres for video: {0}", item.Name);
            _libraryManager.UpdateItem(item, item, ItemUpdateType.MetadataEdit, null);
#else
            _logger.LogInformation("OrganizeGenres for video: {Name}", item.Name);
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