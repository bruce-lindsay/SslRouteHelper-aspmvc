using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using SslHelpers;

namespace SslHelpers.Test
{
    [TestFixture]
    public class SslRouteUrlHelperTests
    {
        private RouteOptions _routeOptions;
        private RouteCollection _routeCollection;
        private UrlHelper _urlHelper;

        private void AssertUriPathMatchesOption(Ssl? ssl, string path)
        {
            if (path == null)
            {
                Assert.Fail("Uri path was null");
            }

            if (ssl.HasValue)
            {
                var expectedScheme = (ssl==Ssl.Add) ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
                Assert.That(path.StartsWith(expectedScheme + ":"), path);
            }
            else
            {
                Assert.That(path.StartsWith("/"), path);
            }
        }

        [SetUp]
        public void DoSetup()
        {
            OptionReset.Do();
            _routeOptions = new RouteOptions();
            _routeCollection = new RouteCollection();

            var requestContextRouteData = new RouteData();
            requestContextRouteData.Values["controller"] = "unittest";

            var requestContext = new RequestContext(new FakeHttpContextBase(), requestContextRouteData);
            _urlHelper = new UrlHelper(requestContext, _routeCollection);
        }

        [TestCase(Ssl.Add)]
        [TestCase(Ssl.Remove)]
        public void NamedRouteSetForSsl_UrlProtocolIsHttps(Ssl ssl)
        {
            _routeCollection.MapRoute(ssl, "Route1", "anything");
            var sslRouteByName = SslHelpers.UrlHelpers.SslRouteUrl(_urlHelper, "Route1");

            AssertUriPathMatchesOption(ssl, sslRouteByName);
        }

        [TestCase(Ssl.Add)]
        [TestCase(Ssl.Remove)]
        [TestCase(null)]
        public void UnconfiguredNamedRoute_UrlProtocolMatchesGivenDefault(Ssl? defaultValue)
        {
            _routeOptions.Default = defaultValue;

            _routeCollection.MapRoute("Route1", "anything");

            var sslRouteByName = SslHelpers.UrlHelpers.SslRouteUrl(_urlHelper, "Route1");

            AssertUriPathMatchesOption(defaultValue, sslRouteByName);
        }

        [TestCase(Ssl.Add)]
        [TestCase(Ssl.Remove)]
        public void ControllerSetForProtocol_ControllerInParameters_ProtocolIsRendered(Ssl option)
        {
            _routeOptions.SetOptionByController(option, "mycontroller");
            _routeCollection.MapRoute("etcroutes", "etc/{controller}/{action}");
            var actual = SslHelpers.UrlHelpers.SslRouteUrl(_urlHelper, "etcroutes", new { controller = "mycontroller", action ="someaction" });

            AssertUriPathMatchesOption(option, actual);
        }

        [TestCase(Ssl.Add)]
        [TestCase(Ssl.Remove)]
        public void ControllerSetForProtocol_ControllerInContext_ProtocolIsRendered(Ssl option)
        {
            //NOTE: "unittest" is the route value from request context, assigned at setup
            _routeOptions.SetOptionByController(option, "unittest");

            _routeCollection.MapRoute("etcroutes", "etc/{controller}/{action}");
            var actual = SslHelpers.UrlHelpers.SslRouteUrl(_urlHelper, "etcroutes", new { action = "someaction" });

            AssertUriPathMatchesOption(option, actual);
        }

        [TestCase(Ssl.Add, Ssl.Remove)]
        [TestCase(Ssl.Remove, Ssl.Add)]
        public void ControllerSetForProtocol_DefaultExists_ControllerOverridesDefault(Ssl controllerSetting, Ssl @default)
        {
            _routeOptions.SetOptionByController(controllerSetting, "armada");
            _routeCollection.MapRoute("etcroutes", "etc/{controller}/{action}");
            _routeOptions.Default = @default;

            var actual = SslHelpers.UrlHelpers.SslRouteUrl(_urlHelper, "etcroutes", new { controller = "armada", action = "someaction" });
            AssertUriPathMatchesOption(controllerSetting, actual);
        }

        [Test]
        public void ControllerSetForProtocol_ParamCaseMismatch_ProtocolIsUsed()
        {
            _routeOptions.SetOptionByController(Ssl.Add, "armada");
            _routeCollection.MapRoute("etcroutes", "etc/{controller}/{action}");
            
            AssertUriPathMatchesOption(Ssl.Add,
                UrlHelpers.SslRouteUrl(_urlHelper, "etcroutes", new { controller = "ARMADA", action ="someaction" }));
        }

        [Test]
        public void ControllerSetForProtocol_ContextCaseMismatch_ProtocolIsUsed()
        {
            _routeOptions.SetOptionByController(Ssl.Remove, "UNiTTeST");
            _routeCollection.MapRoute("etcroutes", "etc/{controller}/{action}");

            AssertUriPathMatchesOption(Ssl.Remove,
                UrlHelpers.SslRouteUrl(_urlHelper, "etcroutes", new { action = "someaction" }));
        }
        /*
        [Test]
        public void VerifyMvcHostNameBehavior()
        {
            _routeCollection.MapRoute("swill", "budwisener");

            Assert.That(
                _urlHelper.RouteUrl("swill", new RouteValueDictionary(new { twoplustwo = "four" }), "ftp", "beerme.net"),
                Is.EqualTo("ftp://beerme.net/budwisener?twoplustwo=four"));

            Assert.That(
                _urlHelper.RouteUrl("swill", null, "ftp", "beerme.net"),
                Is.EqualTo("ftp://beerme.net/budwisener"));

            Assert.That(
                _urlHelper.RouteUrl("swill", null, null),
                Is.EqualTo("/budwisener"));
        }
        */
        [Test]
        public void AlternateHostnamePassed_ParamConfiguredForSsl_UrlIsSslForAlternateHostname()
        {
            _routeCollection.MapRoute(Ssl.Add, "igloo", "igloo");
            Assert.That(
                _urlHelper.SslRouteUrl("igloo", null, "eskimo.example.net"),
                Is.EqualTo("https://eskimo.example.net/igloo"));
        }

        private class OptionReset : RouteOptions
        {
            public static void Do()
            {
                RouteOptions._CurrentOptions = new RouteOptionCollection();
            }
        }
    }
}
