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
        private static Dictionary<string, bool> _configuredRoutes = new Dictionary<string, bool>();
        private static Dictionary<string, bool> _configuredControllers = new Dictionary<string, bool>();

        private static void SetConfiguration(this Dictionary<string, bool> configDict, string key, Ssl sslOption)
        {
            configDict[key] = sslOption == Ssl.Add;
        }

        public static bool? useSsl(string name)
        {
            string lowerName = name.ToLowerInvariant();
            if (_configuredRoutes.ContainsKey(lowerName)) return _configuredRoutes[lowerName];
            return null;
        }

        public static void configureRoute(string name, Ssl ssl)
        {
            _configuredRoutes.SetConfiguration(name, ssl);
        }

        public static void SetOptionByController(string controllerName, Ssl ssl)
        {
            _configuredControllers.SetConfiguration(controllerName, ssl);
        }

        internal static bool? controllerUseSsl(string controllerName)
        {
            if(!String.IsNullOrWhiteSpace(controllerName))
            {
                string nameLower = controllerName.ToLowerInvariant();
                if (_configuredControllers.ContainsKey(nameLower))
                    return _configuredControllers[nameLower];
            }
            return null;           
        }
    }
}