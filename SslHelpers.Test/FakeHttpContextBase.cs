using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace SslHelpers.Test
{
    internal class FakeHttpContextBase : HttpContextBase
    {
        public const string REQUEST_DOMAIN = "abc.example.net";
        public const string REQUEST_URL = "http://abc.example.net/def/ghi";

        private FakeRequestBase _requestBase;
        private FakeResponseBase _responseBase;

        public FakeHttpContextBase()
        {
            _requestBase = new FakeRequestBase();
            _responseBase = new FakeResponseBase();
        }

        private class FakeRequestBase : HttpRequestBase
        {
            public override string ApplicationPath
            {
                get
                {
                    return String.Empty;
                }
            }

            public override Uri Url
            {
                get
                {
                    return new Uri(REQUEST_URL);
                }
            }

            public override System.Collections.Specialized.NameValueCollection ServerVariables
            {
                get
                {
                    return null;
                }
            }
        }
        private class FakeResponseBase : HttpResponseBase
        {
            private string _appPath;
            public FakeResponseBase(string appPath = "")
            {
                _appPath = appPath;
            }
            public override string ApplyAppPathModifier(string virtualPath)
            {
                return _appPath + (!String.IsNullOrEmpty(_appPath) ? "/" : String.Empty) + virtualPath;
            }
        }

        public override HttpRequestBase Request
        {
            get
            {
                return this._requestBase;
            }
        }

        public override HttpResponseBase Response
        {
            get
            {
                return this._responseBase;
            }
        }
    }
}
