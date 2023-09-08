﻿using EventManagement.Models;
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
    [Authorize]
    public class EventsController : Controller
    {
        EventManagement1Entities2 EventManagementEntities = new EventManagement1Entities2();


        // GET: Events
        [Authorize(Roles = "User")]
        [AllowAnonymous]
        public ActionResult EventsName()
        {
            List<EventName> events = EventManagementEntities.EventNames.ToList();
            return View(events);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<EventName> events = EventManagementEntities.EventNames.ToList();
            return View(events);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EventsCreate(int? id)
        {
            EventName events = EventManagementEntities.EventNames.Find(id);
            return View(events);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EventsCreate(HttpPostedFileBase eventimage, [Bind(Include = "eventname1")] EventName eventnames)
        {
            if (ModelState.IsValid)
            {
                byte[] profile;

                using (var reader = new BinaryReader(eventimage.InputStream))
                {
                    profile = reader.ReadBytes(eventimage.ContentLength);
                }
                eventnames.eventimage = profile;

                int lastUserId = EventManagementEntities.EventNames.Max(u => u.eventid);

                eventnames.eventid = lastUserId + 1;


                EventManagementEntities.EventNames.Add(eventnames);
                EventManagementEntities.SaveChanges();
                RedirectToAction("Index");
            }
            return View();
        }






        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult EventDirect(int? eventid)
        {
           if(eventid == 1)
            {
                TempData["eventid"] = eventid;

                return RedirectToAction("BirthdayCreate", "Birthday");

            }
            if (eventid == 2)
            {
                TempData["eventid"] = eventid;

                return RedirectToAction("BabyshowerCreate", "Babyshower");
            }

            return Content("Not a Valid event id");
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            EventName eventstable = EventManagementEntities.EventNames.Find(id);
            return View(eventstable);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult EventDeleteConfirmed(int? id)
        {
            EventName tr = EventManagementEntities.EventNames.Find(id);
            EventManagementEntities.EventNames.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            EventName eventstable = EventManagementEntities.EventNames.Find(id);
            return View(eventstable);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            EventName eventstable = EventManagementEntities.EventNames.Find(id);
            return View(eventstable);
        }

        [HttpPost]
        public ActionResult Edit(int? id, HttpPostedFileBase eventimage,[Bind(Include = "eventname1,eventd")] EventName updateevent)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EventName existingUser = EventManagementEntities.EventNames.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (eventimage != null && eventimage.ContentLength > 0)
                {
                    byte[] profile;

                    using (var reader = new BinaryReader(eventimage.InputStream))
                    {
                        profile = reader.ReadBytes(eventimage.ContentLength);
                    }

                    existingUser.eventimage = profile;
                }

                existingUser.eventname1 = updateevent.eventname1;
                existingUser.eventd = updateevent.eventd;



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