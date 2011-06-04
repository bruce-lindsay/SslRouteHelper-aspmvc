namespace System.Web.WebPages {
    using System.Web.Mvc;
    public static class SslHelpers {
        public static string SslRouteUrl(this UrlHelper urlHelper, string routeName) {
            bool? useSsl = SslRouteHelpers.useSsl(routeName);
            if(useSsl == true) {
                return urlHelper.RouteUrl(routeName, null, "https");
            } else if (useSsl == false) {
                return urlHelper.RouteUrl(routeName, null, "http");
            }
            return urlHelper.RouteUrl(routeName);
        }
    }
}

namespace System.Web.Mvc {
    using System.Collections.Generic;
    using System.Web.Routing;

    public enum Ssl {
        Prefer,
        Avoid
    }

    public static class SslRouteHelpers {
        private static Dictionary<string, bool> _configuredRotues = new Dictionary<string, bool>();

        public static bool? useSsl(string name) {
            if (_configuredRotues.ContainsKey(name)) return _configuredRotues[name];
            return null;
        }

        public static Route MapRoute(this RouteCollection routes, Ssl ssl, string name, string url) {
            configureRoute(name, ssl);
            return routes.MapRoute(name, url);
        }

        private static void configureRoute(string name, Ssl ssl) {
            _configuredRotues[name] = ssl == Ssl.Prefer;
        }
    }
}