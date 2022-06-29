using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.JavTube.Helpers;

public static class GenreHelper
{
    public const string ChineseSubtitle = "中文字幕";

    private static bool HasTag(string filename, string tag)
    {
        var r = new Regex(@"[-_\s]", RegexOptions.Compiled);
        return r.Split(filename).Contains(tag, StringComparer.OrdinalIgnoreCase);
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
        var r = new Regex(@"\.(chinese|ch[ist]|zh(-(cn|hk|tw|hans|hant))?)\.(ass|srt|ssa|stl|sub|vid|vtt)$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        return files.Any(name => r.IsMatch(name) &&
                                 r.Replace(name, string.Empty)
                                     .Equals(basename, StringComparison.OrdinalIgnoreCase));
    }
}