using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using RouteValueDictionary = System.Web.Routing.RouteValueDictionary;

namespace SslHelpers
{
    public static class UrlHelpers
    {
        private static string ProtocolString(Ssl? sslValue)
        {
            if (!sslValue.HasValue)
                return null;

            return (sslValue == Ssl.Add) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }

        private static bool TryGetProtocolDefault(out string protocol)
        {
            Ssl? sslDefault = RouteOptions.Current.Default;

            protocol = ProtocolString(sslDefault);

            return sslDefault.HasValue;
        }

        private static bool TryGetProtocol(string routeName, out string protocol)
        {
            Ssl? useSsl = RouteOptions.Current.GetOptionForNamedRoute(routeName);

            protocol = ProtocolString(useSsl);

            return useSsl.HasValue;
        }

        private static bool TryGetProtocol(RouteValueDictionary routeValues, out string protocol)
        {
            Ssl? ssl = null;

            if ((routeValues != null) && (routeValues.ContainsKey("controller")))
            {
                ssl = RouteOptions.Current.GetOptionForValues(routeValues["controller"] as string);
            }

            protocol = ProtocolString(ssl);

            return ssl.HasValue;
        }

        public static string SslRouteUrl(this UrlHelper urlHelper, string routeName)
        {
            return SslRouteUrl(urlHelper, routeName, null);
        }

        public static string SslRouteUrl(this UrlHelper urlHelper, string routeName, object routeValues)
        {
            return SslRouteUrl(urlHelper, routeName, new RouteValueDictionary(routeValues));
        }

        public static string SslRouteUrl(this UrlHelper urlHelper, string routeName, RouteValueDictionary routeValues)
        {
            return SslRouteUrl(urlHelper, routeName, routeValues, null);
        }

        public static string SslRouteUrl(this UrlHelper urlHelper, string routeName, RouteValueDictionary routeValues,
            string hostName)
        {
            string protocol = null;

            bool foundProtocol = TryGetProtocol(routeName, out protocol)
                || TryGetProtocol(routeValues, out protocol)
                || TryGetProtocol(urlHelper.RequestContext.RouteData.Values, out protocol)
                || TryGetProtocolDefault(out protocol);

            return urlHelper.RouteUrl(routeName, routeValues, protocol, hostName);
        }
    }
}
