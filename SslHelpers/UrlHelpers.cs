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
                protocol = "https";
            }
            else if (useSsl == false)
            {
                getProcotol = true;
                protocol = "http";
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
            string protocol, routeUrl;

            if(TryGetProctocol(routeName, out protocol))
            {
                routeUrl = urlHelper.RouteUrl(routeName, null, protocol);
            }
            else
            {
                routeUrl = urlHelper.RouteUrl(routeName, null);
            }

            return routeUrl;
        }

        public static string SslRouteUrl(this UrlHelper urlHelper, string routeName, RouteValueDictionary routeValues)
        {
            string protocol, routeUrl;

            if(TryGetProctocol(routeName, out protocol))
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
