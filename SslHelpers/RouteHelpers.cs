using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace SslHelpers
{
    public static class RouteHelpers
    {
        public static Route MapRoute(this RouteCollection routes, Ssl ssl, string name, string url)
        {
            RouteOptions.Current.SetOptionForNamedRoute(ssl, name);
            return routes.MapRoute(name, url);
        }
    }
}
