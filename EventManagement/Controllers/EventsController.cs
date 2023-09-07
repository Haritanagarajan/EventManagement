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
    [Authorize]
    public class EventsController : Controller
    {
        EventManagementEntities4     EventManagementEntities = new EventManagementEntities4();


        // GET: Events
        [Authorize(Roles = "User")]
        [AllowAnonymous]
        public ActionResult EventsName()
        {
            List<eventstable> events = EventManagementEntities.eventstables.ToList();
            var eventNamesId = events.Select(e=>e.eventsid).ToList();
            return View(events);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<eventstable> events = EventManagementEntities.eventstables.ToList();
            var eventNamesId = events.Select(e => e.eventsid).ToList();
            return View(events);
        }


        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult EventsCreate(int? id)
        {
            eventstable events = EventManagementEntities.eventstables.Find(id);
            return View(events);
        }


        [HttpPost]
        public ActionResult EventsCreate([Bind(Include = "eventsid,id,eventname")] eventstable eventnames)
        {
            if(ModelState.IsValid)
            {

                int lastUserId = EventManagementEntities.eventstables.Max(u => u.eventsid);

                eventnames.eventsid = lastUserId + 1;

                int lastUserId2 = (int)EventManagementEntities.eventstables.Max(u => u.id);

                eventnames.id = lastUserId2 + 1;

                EventManagementEntities.eventstables.Add(eventnames);
                EventManagementEntities.SaveChanges();
                RedirectToAction("Index");
            }
            return View();
        }

       

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult BookNow()
        {  
            return RedirectToAction("BookingCreate","Booking");
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            eventstable eventstable = EventManagementEntities.eventstables.Find(id);
            return View(eventstable);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult EventDeleteConfirmed(int? id)
        {
            eventstable tr = EventManagementEntities.eventstables.Find(id);
            EventManagementEntities.eventstables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            eventstable eventstable = EventManagementEntities.eventstables.Find(id);
            return View(eventstable);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            eventstable eventstable = EventManagementEntities.eventstables.Find(id);
            return View(eventstable);
        }

        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "eventname,IsDeleted")] eventstable updateevent)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            eventstable existingUser = EventManagementEntities.eventstables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {

                existingUser.eventname = updateevent.eventname;
                existingUser.IsDeleted = updateevent.IsDeleted;



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