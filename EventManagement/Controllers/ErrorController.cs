using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult Error201()
        {
            return View();
        }

        public ActionResult Error204()
        {
            return View();
        }
        public ActionResult Error200()
        {
            return View();
        }
    }
}