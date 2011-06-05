using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SslHelpers
{
    /// <summary>
    /// store/retrieve the configured options
    /// </summary>
    public static class RouteOptions
    {
        private static Dictionary<string, bool> _configuredRotues = new Dictionary<string, bool>();

        public static bool? useSsl(string name)
        {
            if (_configuredRotues.ContainsKey(name)) return _configuredRotues[name];
            return null;
        }

        public static void configureRoute(string name, Ssl ssl) {
            _configuredRotues[name] = ssl == Ssl.Add;
        }
    }
}