using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SslHelpers
{
    public class RouteOptionCollection
    {
        private class OptionDictionary : Dictionary<string, Ssl>
        {
            public OptionDictionary() : base(StringComparer.OrdinalIgnoreCase) { }
            public Ssl? ValueOrNull(string key)
            {
                return this.ContainsKey(key) ? this[key] : (Ssl?) null;
            }
        }

        private readonly OptionDictionary _byNameOptions;
        private readonly OptionDictionary _byValueOptions;

        public Ssl? Default { get; set; }

        public RouteOptionCollection()
        {
            _byNameOptions = new OptionDictionary();
            _byValueOptions = new OptionDictionary();
            Default = null;
        }

        public Ssl? GetOptionForNamedRoute(string NamedRoute)
        {
            return _byNameOptions.ValueOrNull(NamedRoute);
        }

        public void SetOptionForNamedRoute(Ssl ssl, string routeName)
        {
            _byNameOptions[routeName] = ssl;
        }

        public void SetOptionForValues(Ssl ssl, string controller)
        {
            _byValueOptions[controller] = ssl;
        }

        public Ssl? GetOptionForValues(string controller)
        {
            return controller == null ? null : _byValueOptions.ValueOrNull(controller);
        }
    }
}
