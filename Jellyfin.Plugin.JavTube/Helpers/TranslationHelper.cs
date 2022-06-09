using System.Collections.Specialized;
using Jellyfin.Plugin.JavTube.Models;

namespace Jellyfin.Plugin.JavTube.Helpers;

public static class TranslationHelper
{
    private const string AutoLanguage = "auto";

    [Flags]
    public enum Mode
    {
        None,
        Title,
        Tagline,
        TitleTagLine,
        Overview,
        TitleOverview,
        TagLineOverview,
        All
    }

    private static class Engine
    {
        // ReSharper disable ConvertToConstant.Local
        //
        // We are intentionally using 'static readonly' here instead of 'const'.
        // 'const' values would be embedded into each assembly that used them and
        // each consuming assembly would have a different 'string' instance. Using
        // .'static readonly' means that all consumers get these exact same 'string'
        // instance, which means the 'ReferenceEquals' checks below work and allow
        // us to optimize comparisons when these constants are used.
        public static readonly string Baidu = "Baidu";
        public static readonly string Google = "Google";
    }

    private static async Task<string> Translate(string q, string from, string to, CancellationToken cancellationToken)
    {
        var engine = Plugin.Instance.Configuration.TranslationEngine;

        var nv = new NameValueCollection();
        if (string.Equals(engine, Engine.Baidu,
                StringComparison.OrdinalIgnoreCase))
        {
            // Limit Baidu API request rate to 1 rps.
            await Task.Delay(1000, cancellationToken);
            nv.Add(new NameValueCollection
            {
                { "baidu-app-id", Plugin.Instance.Configuration.BaiduAppId },
                { "baidu-app-key", Plugin.Instance.Configuration.BaiduAppKey }
            });
        }
        else if (string.Equals(engine, Engine.Google,
                     StringComparison.OrdinalIgnoreCase))
        {
            // Limit Google API request rate to 10 rps.
            await Task.Delay(100, cancellationToken);
            nv.Add(new NameValueCollection
            {
                { "google-api-key", Plugin.Instance.Configuration.GoogleApiKey }
            });
        }
        else
        {
            throw new ArgumentException($"Invalid translation engine: {engine}");
        }

        return (await ApiClient.GetTranslate(q, from, to, engine, nv, cancellationToken)).TranslatedText;
    }

    public static async Task Translate(MovieInfoModel m, string to, CancellationToken cancellationToken)
    {
        var mode = (Mode)Plugin.Instance.Configuration.TranslationMode;

        if ((mode & Mode.Title) != 0 && !string.IsNullOrWhiteSpace(m.Title))
        {
            m.Title = await Translate(m.Title, AutoLanguage, to, cancellationToken);
        }

        if ((mode & Mode.Tagline) != 0 && !string.IsNullOrWhiteSpace(m.Series))
        {
            m.Series = await Translate(m.Series, AutoLanguage, to, cancellationToken);
        }

        if ((mode & Mode.Overview) != 0 && !string.IsNullOrWhiteSpace(m.Summary))
        {
            m.Summary = await Translate(m.Summary, AutoLanguage, to, cancellationToken);
        }
    }
}