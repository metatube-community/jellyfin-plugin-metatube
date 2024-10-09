using System.Text;
using System.Text.RegularExpressions;
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
            string data = "{\"str\":\""+sourceStr+"\"}";
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

                        Regex regex = new Regex("\"msg\":\\s*\"(.*?)\"");
                        Match match = regex.Match(transStr);
                        if (match.Success)
                        {
                            transStr = match.Groups[1].Value;
                            transStr = Regex.Unescape(transStr);  
                        }
                        else
                        {
                            transStr = sourceStr;
                        }
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