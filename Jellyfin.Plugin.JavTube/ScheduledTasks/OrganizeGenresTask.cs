using System.Text.RegularExpressions;
using Jellyfin.Plugin.JavTube.Extensions;
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

            var genres = item.Genres?.ToList() ?? new List<string>();

            try
            {
                // Add or Remove `ChineseSubtitle` genre.
                if (HasEmbeddedChineseSubtitle(item.FileNameWithoutExtension) ||
                    HasExternalChineseSubtitle(item.Path))
                    genres.Add(ChineseSubtitle);
                else
                    genres.RemoveAll(s => s.Equals(ChineseSubtitle));
            }
            catch (Exception e)
            {
                _logger.Error("Update ChineseSubtitle for video {0}: {1}", item.Name, e.Message);
            }

            // Remove duplicates.
            var orderedGenres =
                (Plugin.Instance.Configuration.EnableGenreSubstitution
                    // Substitute genres.
                    ? genres.Substitute(Plugin.Instance.Configuration.GetGenreSubstitutionTable())
                    : genres).Distinct().OrderByString(genre => genre).ToList();

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

    #region Helper

    private const string ChineseSubtitle = "中文字幕";

    private static bool HasTag(string filename, string tag)
    {
        var r = new Regex(@"[-_\s]", RegexOptions.Compiled);
        return r.Split(filename).Contains(tag, StringComparer.OrdinalIgnoreCase);
    }

    private static bool HasTag(string filename, params string[] tags)
    {
        return tags.Any(tag => HasTag(filename, tag));
    }

    private static bool HasEmbeddedChineseSubtitle(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return false;

        return filename.Contains(ChineseSubtitle) || HasTag(filename, "C", "ch");
    }

    private static bool HasExternalChineseSubtitle(string path)
    {
        return HasExternalChineseSubtitle(Path.GetFileNameWithoutExtension(path),
            Directory.GetParent(path)?.GetFiles().Select(info => info.Name));
    }

    private static bool HasExternalChineseSubtitle(string basename, IEnumerable<string> files)
    {
        var r = new Regex(@"\.(chinese|ch[ist]|zh(-(cn|hk|tw|hans|hant))?)\.(ass|srt|ssa|stl|sub|vid|vtt)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        return files.Any(name => r.IsMatch(name) &&
                                 r.Replace(name, string.Empty)
                                     .Equals(basename, StringComparison.OrdinalIgnoreCase));
    }

    #endregion
}