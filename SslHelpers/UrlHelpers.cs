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
        private static bool TryGetProctocol(string routeName, out string protocol)
        {
            bool getProcotol = false;
            bool? useSsl = RouteOptions.useSsl(routeName);

            if (useSsl == true)
            {
                getProcotol = true;
                protocol = System.Uri.UriSchemeHttps;
            }
            else if (useSsl == false)
            {
                getProcotol = true;
                protocol = System.Uri.UriSchemeHttp;
            }
            else
            {
                protocol = null;
            }

            return getProcotol;
        }

        private static bool TryGetProctocol(RouteValueDictionary routeValues, out string protocol)
        {
            bool getProcotol = false;
            bool? useSsl = null;

            if(routeValues!=null)
                useSsl =  RouteOptions.controllerUseSsl(routeValues["controller"] as string);

            if (useSsl == true)
            {
                getProcotol = true;
                protocol = System.Uri.UriSchemeHttps;
            }
            else if (useSsl == false)
            {
                getProcotol = true;
                protocol = System.Uri.UriSchemeHttp;
            }
            else
            {
                protocol = null;
            }

            return getProcotol;
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
            string protocol, routeUrl;

            if(TryGetProctocol(routeName, out protocol) || TryGetProctocol(routeValues, out protocol))
            {
                routeUrl = urlHelper.RouteUrl(routeName, routeValues, protocol, null);
            }
            else
            {
                routeUrl = urlHelper.RouteUrl(routeName, routeValues);
            }

            return routeUrl;
        }
    }
}
