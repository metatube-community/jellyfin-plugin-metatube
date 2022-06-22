using System.Collections.Specialized;
using Jellyfin.Plugin.JavTube.Configuration;
using Jellyfin.Plugin.JavTube.Models;

namespace Jellyfin.Plugin.JavTube.Helpers;

public static class TranslationHelper
{
    [Flags]
    public enum Mode
    {
        Disabled,
        Title,
        Tagline,
        TitleTagline,
        Overview,
        TitleOverview,
        TaglineOverview,
        All
    }

    private const string AutoLanguage = "auto";

    private static class Engine
    {
        // ReSharper disable ConvertToConstant.Local
        //
        // We are intentionally using 'static readonly' here instead of 'const'.
        // 'const' values would be embedded into each assembly that used them and
        // each consuming assembly would have a different 'string' instance. Using
        // 'static readonly' means that all consumers get these exact same 'string'
        // instance, which means the 'ReferenceEquals' checks below work and allow
        // us to optimize comparisons when these constants are used.
        public static readonly string Baidu = "Baidu";
        public static readonly string Google = "Google";
    }

    private static PluginConfiguration Configuration => Plugin.Instance.Configuration;

    private static readonly SemaphoreSlim Semaphore = new(1);

    private static async Task<string> Translate(string q, string from, string to, CancellationToken cancellationToken)
    {
        int delayInMs;
        var nv = new NameValueCollection();
        if (string.Equals(Configuration.TranslationEngine, Engine.Baidu,
                StringComparison.OrdinalIgnoreCase))
        {
            // Limit Baidu API request rate to 1 rps.
            delayInMs = 1000;
            nv.Add(new NameValueCollection
            {
                { "baidu-app-id", Configuration.BaiduAppId },
                { "baidu-app-key", Configuration.BaiduAppKey }
            });
        }
        else if (string.Equals(Configuration.TranslationEngine, Engine.Google,
                     StringComparison.OrdinalIgnoreCase))
        {
            // Limit Google API request rate to 10 rps.
            delayInMs = 100;
            nv.Add(new NameValueCollection
            {
                { "google-api-key", Configuration.GoogleApiKey }
            });
        }
        else
        {
            throw new ArgumentException($"Invalid translation engine: {Configuration.TranslationEngine}");
        }

        await Semaphore.WaitAsync(cancellationToken);

        try
        {
            async Task<string> TranslateWithDelay()
            {
                await Task.Delay(delayInMs, cancellationToken);
                return (await ApiClient
                    .GetTranslate(q, from, to, Configuration.TranslationEngine, nv, cancellationToken)
                    .ConfigureAwait(false)).TranslatedText;
            }

            return await RetryAsync(5, TranslateWithDelay);
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public static async Task Translate(MovieInfoModel m, string to, CancellationToken cancellationToken)
    {
        var mode = (Mode)Configuration.TranslationMode;

        if ((mode & Mode.Title) != 0 && !string.IsNullOrWhiteSpace(m.Title))
            m.Title = await Translate(m.Title, AutoLanguage, to, cancellationToken);

        if ((mode & Mode.Tagline) != 0 && !string.IsNullOrWhiteSpace(m.Series))
            m.Series = await Translate(m.Series, AutoLanguage, to, cancellationToken);

        if ((mode & Mode.Overview) != 0 && !string.IsNullOrWhiteSpace(m.Summary))
            m.Summary = await Translate(m.Summary, AutoLanguage, to, cancellationToken);
    }

    private static async Task<T> RetryAsync<T>(uint numRetries, Func<Task<T>> func)
    {
        uint numAttempts = 1;
        while (true)
        {
            try
            {
                return await func();
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch
            {
                if (numAttempts < numRetries)
                {
                    ++numAttempts;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}