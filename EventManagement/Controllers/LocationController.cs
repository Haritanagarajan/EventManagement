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


    public class LocationController : Controller
    {

        // GET: location
        EventManagement1Entities2 EventManagementEntities = new EventManagement1Entities2();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<locationtable> locations = EventManagementEntities.locationtables.ToList();
            return View(locations);
        }
        // GET: Location
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public ActionResult LocationCreate(int? id)
        {
            locationtable locations = EventManagementEntities.locationtables.Find(id);
            return View(locations);
        }

        [HttpPost]
        public ActionResult LocationCreate(HttpPostedFileBase locationimage, [Bind(Include = "locationname,pincode")] locationtable locations)
        {
            if (ModelState.IsValid)
            {
                byte[] profile;

                using (var reader = new BinaryReader(locationimage.InputStream))
                {
                    profile = reader.ReadBytes(locationimage.ContentLength);
                }
                locations.locationimage = profile;

                int lastUserId = EventManagementEntities.locationtables.Max(u => u.locationid);

                locations.locationid = lastUserId + 1;
                EventManagementEntities.locationtables.Add(locations);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            locationtable locationtables = EventManagementEntities.locationtables.Find(id);
            return View(locationtables);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult LocationDeleteConfirmed(int? id)
        {
            locationtable tr = EventManagementEntities.locationtables.Find(id);
            EventManagementEntities.locationtables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            locationtable locationtables = EventManagementEntities.locationtables.Find(id);
            return View(locationtables);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            locationtable locationtables = EventManagementEntities.locationtables.Find(id);
            return View(locationtables);
        }

        [HttpPost]
        public ActionResult Edit(int? id, HttpPostedFileBase locationimage,[Bind(Include = "locationname,pincode,locationd")] locationtable updatelocation)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            locationtable existingUser = EventManagementEntities.locationtables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (locationimage != null && locationimage.ContentLength > 0)
                {
                    byte[] profile;

                    using (var reader = new BinaryReader(locationimage.InputStream))
                    {
                        profile = reader.ReadBytes(locationimage.ContentLength);
                    }

                    existingUser.locationimage = profile;
                }

                existingUser.locationd = updatelocation.locationd;
                existingUser.pincode = updatelocation.pincode;



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