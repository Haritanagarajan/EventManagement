using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    public class DecorationController : Controller
    {
        // GET: Decoration
        // GET: location
        EventManagement1Entities3 EventManagementEntities = new EventManagement1Entities3();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<decorationtable> decorationtable = EventManagementEntities.decorationtables.ToList();
            return View(decorationtable);
        }
        // GET: Location
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public ActionResult DecorCreate(int? id)
        {
            decorationtable decorationtable = EventManagementEntities.decorationtables.Find(id);
            return View(decorationtable);
        }

        [HttpPost]
        public ActionResult DecorCreate(HttpPostedFileBase decorimage, [Bind(Include = "decoravailable")] decorationtable decorationtables)
        {
            if (ModelState.IsValid)
            {
                byte[] profile;

                using (var reader = new BinaryReader(decorimage.InputStream))
                {
                    profile = reader.ReadBytes(decorimage.ContentLength);
                }
                decorationtables.decorimage = profile;

                int lastUserId = EventManagementEntities.decorationtables.Max(u => u.decorid);

                decorationtables.decorid = lastUserId + 1;
                EventManagementEntities.decorationtables.Add(decorationtables);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            decorationtable decorationtable = EventManagementEntities.decorationtables.Find(id);
            return View(decorationtable);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DecorDeleteConfirmed(int? id)
        {
            decorationtable tr = EventManagementEntities.decorationtables.Find(id);
            EventManagementEntities.decorationtables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            decorationtable decorationtable = EventManagementEntities.decorationtables.Find(id);
            return View(decorationtable);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            decorationtable decorationtable = EventManagementEntities.decorationtables.Find(id);
            return View(decorationtable);
        }

        [HttpPost]
        public ActionResult Edit(int? id, HttpPostedFileBase decorimage, [Bind(Include = "decoravailable,decord")] decorationtable updatedecor)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            decorationtable existingUser = EventManagementEntities.decorationtables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (decorimage != null && decorimage.ContentLength > 0)
                {
                    byte[] profile;

                    using (var reader = new BinaryReader(decorimage.InputStream))
                    {
                        profile = reader.ReadBytes(decorimage.ContentLength);
                    }

                    existingUser.decorimage = profile;
                }

                existingUser.decoravailable = updatedecor.decoravailable;
                existingUser.decord = updatedecor.decord;



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