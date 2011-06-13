using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SslHelpers
{
    /// <summary>
    /// store/retrieve the configured options
    /// </summary>
    public class RouteOptions
    {
        protected internal static RouteOptionCollection _CurrentOptions = new RouteOptionCollection();

        internal static RouteOptionCollection Current { get { return _CurrentOptions; } }

        public void SetOptionByController(Ssl ssl, string controller)
        {
            _CurrentOptions.SetOptionForValues(ssl, controller);
        }
        public Ssl? Default 
        {
            get { return _CurrentOptions.Default; }
            set { _CurrentOptions.Default = value; }
        }
    }
}