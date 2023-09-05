using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    public class BookingController : Controller
    {
        EventManagementEntities1 EventManagementEntities = new EventManagementEntities1();

        // GET: Booking
        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult BookingCreate()
        {
            List<datetable>  date = EventManagementEntities.datetables.ToList();
            ViewBag.Date = new SelectList(date, "dateid", "datesavailable");
            List<timetable> time = EventManagementEntities.timetables.ToList();
            ViewBag.Time = new SelectList(time, "timeid", "timesavailable");
            List<eventstable> eventsname = EventManagementEntities.eventstables.ToList();
            ViewBag.Events = new SelectList(eventsname, "eventsid", "eventname");
            List<locationtable> location = EventManagementEntities.locationtables.ToList();
            ViewBag.Location = new SelectList(location, "locationid", "locationname");
            List<themetable> theme = EventManagementEntities.themetables.ToList();
            ViewBag.Theme = new SelectList(theme, "themeid", "themename");
            return View();
        }
        [HttpPost]
        public ActionResult BookingCreate([Bind(Include = "eventid,eventdate,eventtime,eventcatering,eventlocation,eventtheme,eventcost")]eventname names)
        {
            if (ModelState.IsValid)
            {
                int lastUserId = (int)EventManagementEntities.eventnames.Max(u => u.eventnameid);

                names.eventnameid = lastUserId + 1;

                names.eventhallcapacity = 500;

                EventManagementEntities.eventnames.Add(names);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("BookingDetails",new {eventnameid=names.eventnameid});
            }
            return View();
        }

        
        [HttpGet]
        [Authorize(Roles ="User")]
        public ActionResult BookingDetails(int? eventnameid)
        {
           //eventname  eventname = EventManagementEntities.eventnames.Find(eventnameid);
            eventname selectedevent=(from s in EventManagementEntities.eventnames where s.eventnameid==eventnameid select s).FirstOrDefault();


            return View(selectedevent);
        }
    }
}