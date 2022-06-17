using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.JavTube.Helpers;

public static class GenreHelper
{
    public const string ChineseSubtitle = "中文字幕";

    public static readonly Dictionary<string, string> SubstitutionTable =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "HD", null },
            { "4K", null },
            { "5K", null },
            { "720p", null },
            { "1080p", null },
            { "60fps", null }
        };

    private static bool HasTag(string filename, string tag)
    {
        var r = new Regex(@"[-_\s]", RegexOptions.Compiled);
        return r.Split(filename).ToList().Contains(tag, StringComparer.OrdinalIgnoreCase);
    }

    private static bool HasTag(string filename, params string[] tags)
    {
        return tags.Any(tag => HasTag(filename, tag));
    }

    public static bool HasEmbeddedChineseSubtitle(string filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return false;

        return filename.Contains(ChineseSubtitle) || HasTag(filename, "C", "ch");
    }

    public static bool HasExternalChineseSubtitle(string path)
    {
        return HasExternalChineseSubtitle(Path.GetFileNameWithoutExtension(path),
            Directory.GetParent(path)?.GetFiles().Select(info => info.Name));
    }

    private static bool HasExternalChineseSubtitle(string basename, IEnumerable<string> files)
    {
        var r = new Regex(@"\.(chinese|chi|chs|cht)\.(ass|srt|ssa|stl|sub|vid|vtt)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        return files.Any(name => r.IsMatch(name) &&
                                 r.Replace(name, string.Empty)
                                     .Equals(basename, StringComparison.OrdinalIgnoreCase));
    }
}