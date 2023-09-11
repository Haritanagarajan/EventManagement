using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace EventManagement.Controllers
{


    public class ThemeController : Controller
    {
        // GET: theme
        EventManagement1Entities3 EventManagementEntities = new EventManagement1Entities3();

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
        public ActionResult ThemeCreate(HttpPostedFileBase themeimage,[Bind(Include = "themename")] themetable themes)
        {
            if (ModelState.IsValid)
            {
                byte[] profile;

                using (var reader = new BinaryReader(themeimage.InputStream))
                {
                    profile = reader.ReadBytes(themeimage.ContentLength);
                }
                themes.themeimage = profile;

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
        public ActionResult Edit(int? id, HttpPostedFileBase themeimage ,[Bind(Include = "themename,themed")] themetable updatetheme)
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
                if (themeimage != null && themeimage.ContentLength > 0)
                {
                    byte[] profile;

                    using (var reader = new BinaryReader(themeimage.InputStream))
                    {
                        profile = reader.ReadBytes(themeimage.ContentLength);
                    }

                    existingUser.themeimage = profile;
                }

                existingUser.themename = updatetheme.themename;
                existingUser.themed = updatetheme.themed;



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