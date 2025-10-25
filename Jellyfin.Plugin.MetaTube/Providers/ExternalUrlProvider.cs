#if !__EMBY__
using Jellyfin.Plugin.MyTube.ExternalIds;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.MyTube.Providers
{
    public class ExternalUrlProvider : IExternalUrlProvider
    {
        public string Name => Plugin.Instance.Name;

        public IEnumerable<string> GetExternalUrls(BaseItem item)
        {
            if (item.TryGetProviderId(Plugin.Instance.Name, out var pid))
            {
                switch (item)
                {
                    case Movie:
                        yield return string.Format(new MovieExternalId().UrlFormatString, pid);
                        break;

                    case Person:
                        yield return string.Format(new ActorExternalId().UrlFormatString, pid);
                        break;
                }
            }
            
        }
    }
}
#endif
