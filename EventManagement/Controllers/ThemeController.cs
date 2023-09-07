using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
 

    public class ThemeController : Controller
    {
        // GET: theme
        EventManagementEntities4 EventManagementEntities = new EventManagementEntities4();

        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            List<themetable> themes = EventManagementEntities.themetables.ToList();
            return View(themes);
        }

        [Authorize(Roles = "Admin")]

        // GET: Date
        [HttpGet]
        public ActionResult ThemeCreate(int? id)
        {
            themetable themes = EventManagementEntities.themetables.Find(id);
            return View(themes);
        }

        [HttpPost]
        public ActionResult ThemeCreate([Bind(Include = "themename")] themetable themes)
        {
            if (ModelState.IsValid)
            {
                int lastUserId = EventManagementEntities.themetables.Max(u => u.themeid);

                themes.themeid = lastUserId + 1;
                EventManagementEntities.themetables.Add(themes);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            themetable theme = EventManagementEntities.themetables.Find(id);
            return View(theme);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ThemeDeleteConfirmed(int? id)
        {
            themetable tr = EventManagementEntities.themetables.Find(id);
            EventManagementEntities.themetables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            themetable theme = EventManagementEntities.themetables.Find(id);
            return View(theme);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            themetable theme = EventManagementEntities.themetables.Find(id);
            return View(theme);
        }

        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "themename,IsDeletedtheme")] themetable updatetheme)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            themetable existingUser = EventManagementEntities.themetables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {

                existingUser.themename = updatetheme.themename;
                existingUser.IsDeletedtheme = updatetheme.IsDeletedtheme;



                EventManagementEntities.Entry(existingUser).State = EntityState.Modified;

                try
                {
                    EventManagementEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                }
            }

            return View(existingUser);
        }

    }
}