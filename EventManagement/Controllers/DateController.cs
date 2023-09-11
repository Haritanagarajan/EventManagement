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
   
    public class DateController : Controller
    {
        EventManagement1Entities3 EventManagementEntities = new EventManagement1Entities3();

        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            List<datetable> dates = EventManagementEntities.datetables.ToList();
            return View(dates);
        }
        // GET: Date
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public ActionResult DateCreate(int? id)
        {
            datetable dates = EventManagementEntities.datetables.Find(id);
            return View(dates);
        }

        [HttpPost]
        public ActionResult DateCreate([Bind(Include = "dateid,datesavailable,dated")] datetable dates)
        {
            if(ModelState.IsValid)
            {
                int lastUserId = EventManagementEntities.datetables.Max(u => u.dateid);

                dates.dateid = lastUserId + 1;
                EventManagementEntities.datetables.Add(dates);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            datetable date = EventManagementEntities.datetables.Find(id);
            return View(date);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DateDeleteConfirmed(int? id)
        {
            datetable tr = EventManagementEntities.datetables.Find(id);
            EventManagementEntities.datetables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            datetable date = EventManagementEntities.datetables.Find(id);
            return View(date);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            datetable date = EventManagementEntities.datetables.Find(id);
            return View(date);
        }

        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "datesavailable,dated")] datetable updatedate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            datetable existingUser = EventManagementEntities.datetables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {

                existingUser.datesavailable = updatedate.datesavailable;
                existingUser.dated = updatedate.dated;



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