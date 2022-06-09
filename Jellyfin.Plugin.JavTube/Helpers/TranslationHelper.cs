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
        Overview,
        TitleOverview
    }

    public static class Engine
    {
        public const string Baidu = "Baidu";
        public const string Google = "Google";
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

    public static async void Translate(MovieInfoModel m, string to, CancellationToken cancellationToken)
    {
        var mode = (Mode)Plugin.Instance.Configuration.TranslationMode;

        if ((mode & Mode.Title) != 0 && !string.IsNullOrWhiteSpace(m.Title))
        {
            m.Title = await Translate(m.Title, AutoLanguage, to, cancellationToken);
        }

        if ((mode & Mode.Overview) != 0 && !string.IsNullOrWhiteSpace(m.Summary))
        {
            m.Summary = await Translate(m.Summary, AutoLanguage, to, cancellationToken);
        }
    }
}