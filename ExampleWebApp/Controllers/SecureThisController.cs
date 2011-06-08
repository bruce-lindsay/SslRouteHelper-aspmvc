using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExampleWebApp.Controllers
{
    public class SecureThisController : Controller
    {
        //
        // GET: /SecureThis/

        public ActionResult Index()
        {
            return new EmptyResult();
        }

        public ActionResult NonIndex()
        {
            return new EmptyResult();
        }
    }
}
