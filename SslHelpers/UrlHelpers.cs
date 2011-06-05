using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace SslHelpers
{
    public static class UrlHelpers
    {
        public static string SslRouteUrl(this UrlHelper urlHelper, string routeName)
        {
            bool? useSsl = RouteOptions.useSsl(routeName);
            if (useSsl == true)
            {
                return urlHelper.RouteUrl(routeName, null, "https");
            }
            else if (useSsl == false)
            {
                return urlHelper.RouteUrl(routeName, null, "http");
            }
            return urlHelper.RouteUrl(routeName);
        }
    }
}
