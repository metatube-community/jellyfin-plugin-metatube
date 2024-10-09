using System.Text;
using Jellyfin.Plugin.MetaTube.Configuration;
using Jellyfin.Plugin.MetaTube.Providers;

namespace Jellyfin.Plugin.MetaTube.Translation
{
    public static class TranslationGPT
    {
        private static PluginConfiguration Cfg => Plugin.Instance.Configuration;

        public static async Task<string> TranslationAsync(string sourceStr, CancellationToken cancellationToken, MovieProvider logger)
        {
            if (string.IsNullOrWhiteSpace(sourceStr))
                return sourceStr;
            string apiUrl = Cfg.GPTTranslationUrl;
            string data = $"{{\"str\":\"{sourceStr}\",}}";
            string transStr = sourceStr;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                    HttpResponseMessage res = await httpClient.PostAsync(apiUrl, content, cancellationToken);
                    if (res.IsSuccessStatusCode)
                    {
                        transStr = await res.Content.ReadAsStringAsync(cancellationToken);
                    }
                }
            }
            catch (System.Exception)
            {

            }

            return transStr;
        }
    }
    }