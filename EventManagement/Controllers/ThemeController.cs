using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
 

    public class ThemeController : Controller
    {
        // GET: theme
        EventManagementEntities3 EventManagementEntities3 = new EventManagementEntities3();

        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            List<themetable> themes = EventManagementEntities3.themetables.ToList();
            return View(themes);
        }

        [Authorize(Roles = "Admin")]

        // GET: Date
        [HttpGet]
        public ActionResult ThemeCreate(int? id)
        {
            themetable themes = EventManagementEntities3.themetables.Find(id);
            return View(themes);
        }

        [HttpPost]
        public ActionResult ThemeCreate([Bind(Include = "themeid,themename")] themetable themes)
        {
            if (ModelState.IsValid)
            {
                EventManagementEntities3.themetables.Add(themes);
                EventManagementEntities3.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}