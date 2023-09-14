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


    public class TimeController : Controller
    {
        // GET: Time
        EventManagement2Entities1 EventManagementEntities = new EventManagement2Entities1();

        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            List<timetable> times = EventManagementEntities.timetables.ToList();
            return View(times);
        }
        // GET: Date
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public ActionResult TimeCreate(int? id)
        {
            timetable times = EventManagementEntities.timetables.Find(id);
            return View(times);
        }

        [HttpPost]
        public ActionResult TimeCreate([Bind(Include = "timeid,timesavailable,timed")] timetable timess)
        {
            if (ModelState.IsValid)
            {
                int lastUserId = EventManagementEntities.timetables.Max(u => u.timeid);

                timess.timeid = lastUserId + 1;
                EventManagementEntities.timetables.Add(timess);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            timetable time = EventManagementEntities.timetables.Find(id);
            return View(time);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult TimeDeleteConfirmed(int? id)
        {
            timetable tr = EventManagementEntities.timetables.Find(id);
            EventManagementEntities.timetables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            timetable time = EventManagementEntities.timetables.Find(id);
            return View(time);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            timetable time = EventManagementEntities.timetables.Find(id);

            return View(time);
        }

        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "timesavailable,timed")] timetable updatetime)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            timetable existingUser = EventManagementEntities.timetables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {

                existingUser.timesavailable = updatetime.timesavailable;
                existingUser.timed = updatetime.timed;
           

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