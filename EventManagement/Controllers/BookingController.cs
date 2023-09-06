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
        EventManagementEntities4 EventManagementEntities = new EventManagementEntities4();


        // GET: Booking
        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult BookingCreate()
        {

            
                List<datetable> date = EventManagementEntities.datetables.ToList();
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
        public ActionResult BookingCreate([Bind(Include = "eventid,eventdate,eventtime,eventcatering,eventlocation,eventtheme")] eventname names)
        {
            if (ModelState.IsValid)
            {
                int lastUserId = EventManagementEntities.eventnames.Any() ? EventManagementEntities.eventnames.Max(u => u.eventnameid) : 0;
                int? userId = TempData["UserId"] as int?;

                if (userId.HasValue )
                {
                    names.TUserid = userId.Value;
                }   

                names.eventnameid = lastUserId + 1;
                names.eventhallcapacity = 500;
                names.eventcost = 1000;
                
                EventManagementEntities.eventnames.Add(names);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("BookingDetails", new { eventnameid = names.eventnameid });
            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles ="User")]
        public ActionResult BookingDetails(int? eventnameid)
        {

            try
            {
                eventname selectedevent = (from s in EventManagementEntities.eventnames where s.eventnameid == eventnameid select s).FirstOrDefault();
                return View(selectedevent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpStatusCodeResult(404, "Error in eventnameid" + ex.Message);
            }
            //eventname  eventname = EventManagementEntities.eventnames.Find(eventnameid);


           
        }
    }
}