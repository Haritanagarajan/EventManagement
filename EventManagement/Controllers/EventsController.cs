﻿using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        EventManagementEntities EventManagementEntities = new EventManagementEntities();


        // GET: Events
        [Authorize(Roles = "User")]

        public ActionResult EventsName()
        {
            List<eventstable> events = EventManagementEntities.eventstables.ToList();
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
        public ActionResult EventsCreate([Bind(Include = "eventname")] eventstable eventname)
        {
            if(ModelState.IsValid)
            {
                EventManagementEntities.eventstables.Add(eventname);
                EventManagementEntities.SaveChanges();
                RedirectToAction("StatusBooked");
            }
            return View();
        }

       

        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult StatusBooked(int? id)
        {
            eventstable events = EventManagementEntities.eventstables.Find(id);
            return View(events);
        }


        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult BookNow()
        {  
            return RedirectToAction("BookingCreate","Booking");
        }


    }
}