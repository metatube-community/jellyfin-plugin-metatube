using System.Collections.Specialized;
using Jellyfin.Plugin.JavTube.Configuration;
using Jellyfin.Plugin.JavTube.Metadata;

namespace Jellyfin.Plugin.JavTube.Translation;

public static class TranslationHelper
{
    private const string AutoLanguage = "auto";

    private static readonly SemaphoreSlim Semaphore = new(1);

    private static PluginConfiguration Configuration => Plugin.Instance.Configuration;

    private static async Task<string> TranslateAsync(string q, string from, string to,
        CancellationToken cancellationToken)
    {
        int millisecondsDelay;
        var nv = new NameValueCollection();
        switch (Configuration.TranslationEngine)
        {
            case TranslationEngine.Baidu:
                millisecondsDelay = 1000; // Limit Baidu API request rate to 1 rps.
                nv.Add(new NameValueCollection
                {
                    { "baidu-app-id", Configuration.BaiduAppId },
                    { "baidu-app-key", Configuration.BaiduAppKey }
                });
                break;
            case TranslationEngine.Google:
                millisecondsDelay = 100; // Limit Google API request rate to 10 rps.
                nv.Add(new NameValueCollection
                {
                    { "google-api-key", Configuration.GoogleApiKey }
                });
                break;
            default:
                throw new ArgumentException($"Invalid translation engine: {Configuration.TranslationEngine}");
        }

        await Semaphore.WaitAsync(cancellationToken);

        try
        {
            async Task<string> TranslateWithDelay()
            {
                await Task.Delay(millisecondsDelay, cancellationToken);
                return (await ApiClient
                    .TranslateAsync(q, from, to, Configuration.TranslationEngine.ToString(), nv, cancellationToken)
                    .ConfigureAwait(false)).TranslatedText;
            }

            return await RetryAsync(TranslateWithDelay, 5);
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public static async Task TranslateAsync(MovieInfo m, string to, CancellationToken cancellationToken)
    {
        if (Configuration.TranslationMode.HasFlag(TranslationMode.Title) && !string.IsNullOrWhiteSpace(m.Title))
            m.Title = await TranslateAsync(m.Title, AutoLanguage, to, cancellationToken);

        if (Configuration.TranslationMode.HasFlag(TranslationMode.Summary) && !string.IsNullOrWhiteSpace(m.Summary))
            m.Summary = await TranslateAsync(m.Summary, AutoLanguage, to, cancellationToken);
    }

    private static async Task<T> RetryAsync<T>(Func<Task<T>> func, int retryCount)
    {
        while (true)
        {
            try
            {
                return await func();
            }
            catch when (--retryCount > 0)
            {
            }
        }
    }
}