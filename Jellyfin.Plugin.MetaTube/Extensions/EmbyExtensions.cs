#if __EMBY__

using System.Text;
using MediaBrowser.Model.Logging;

namespace Jellyfin.Plugin.MetaTube.Extensions;

public static class EmbyExtensions
{
    #region LogManager

    public static ILogger CreateLogger<T>(this ILogManager logManager)
    {
        return logManager.GetLogger($"{Plugin.Instance.Name}.{typeof(T).Name}");
    }

    #endregion

    #region Sorting

    public static IEnumerable<T> OrderByString<T>(this IEnumerable<T> list, Func<T, string> getName)
    {
        return list.OrderBy(getName, new AlphanumComparator());
    }

    public static IEnumerable<T> OrderByStringDescending<T>(
        this IEnumerable<T> list,
        Func<T, string> getName)
    {
        return list.OrderByDescending(getName, new AlphanumComparator());
    }

    public static IOrderedEnumerable<T> ThenByString<T>(
        this IOrderedEnumerable<T> list,
        Func<T, string> getName)
    {
        return list.ThenBy(getName, new AlphanumComparator());
    }

    public static IOrderedEnumerable<T> ThenByStringDescending<T>(
        this IOrderedEnumerable<T> list,
        Func<T, string> getName)
    {
        return list.ThenByDescending(getName, new AlphanumComparator());
    }

    private sealed class AlphanumComparator : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return CompareValues(x, y);
        }

        private static bool InChunk(char ch, char otherCh)
        {
            var chunkType = ChunkType.Alphanumeric;
            if (char.IsDigit(otherCh))
                chunkType = ChunkType.Numeric;
            return (chunkType != ChunkType.Alphanumeric || !char.IsDigit(ch)) &&
                   (chunkType != ChunkType.Numeric || char.IsDigit(ch));
        }

        private static int CompareValues(string s1, string s2)
        {
            if (s1 == null || s2 == null)
                return 0;
            var index1 = 0;
            var index2 = 0;
            while (index1 < s1.Length || index2 < s2.Length)
            {
                if (index1 >= s1.Length)
                    return -1;
                if (index2 >= s2.Length)
                    return 1;
                var ch1 = s1[index1];
                var ch2 = s2[index2];
                var stringBuilder1 = new StringBuilder();
                var stringBuilder2 = new StringBuilder();
                while (index1 < s1.Length && (stringBuilder1.Length == 0 || InChunk(ch1, stringBuilder1[0])))
                {
                    stringBuilder1.Append(ch1);
                    ++index1;
                    if (index1 < s1.Length)
                        ch1 = s1[index1];
                }

                while (index2 < s2.Length && (stringBuilder2.Length == 0 || InChunk(ch2, stringBuilder2[0])))
                {
                    stringBuilder2.Append(ch2);
                    ++index2;
                    if (index2 < s2.Length)
                        ch2 = s2[index2];
                }

                var num = 0;
                if (char.IsDigit(stringBuilder1[0]) && char.IsDigit(stringBuilder2[0]))
                {
                    if (!int.TryParse(stringBuilder1.ToString(), out var result1) ||
                        !int.TryParse(stringBuilder2.ToString(), out var result2))
                        return 0;
                    if (result1 < result2)
                        num = -1;
                    if (result1 > result2)
                        num = 1;
                }
                else
                {
                    num = string.Compare(stringBuilder1.ToString(), stringBuilder2.ToString(),
                        StringComparison.CurrentCulture);
                }

                if (num != 0)
                    return num;
            }

            return 0;
        }

        private enum ChunkType
        {
            Alphanumeric,
            Numeric
        }
    }

    #endregion
}

#endif