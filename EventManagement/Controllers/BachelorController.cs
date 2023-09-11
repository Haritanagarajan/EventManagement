using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    public class BachelorController : Controller
    {
        // GET: Bachelor
        EventManagement1Entities3 EventManagementEntities = new EventManagement1Entities3();


        // GET: Booking
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult BachelorCreate()
        {

            List<datetable> date = EventManagementEntities.datetables.ToList();
            ViewBag.Date = new SelectList(date, "dateid", "datesavailable");

            List<timetable> time = EventManagementEntities.timetables.ToList();
            ViewBag.Time = new SelectList(time, "timeid", "timesavailable");

            List<EventName> eventsname = EventManagementEntities.EventNames.ToList();
            ViewBag.Events = new SelectList(eventsname, "eventid", "eventname");

            List<locationtable> location = EventManagementEntities.locationtables.ToList();
            ViewBag.Location = new SelectList(location, "locationid", "locationname");

            List<themetable> theme = EventManagementEntities.themetables.ToList();
            ViewBag.Theme = new SelectList(theme, "themeid", "themename");

            List<decorationtable> decor = EventManagementEntities.decorationtables.ToList();
            ViewBag.Decor = new SelectList(decor, "decorid", "decoravailable");

            List<caketable> cake = EventManagementEntities.caketables.ToList();
            ViewBag.Cake = new SelectList(cake, "cakeid", "cakesavailable");

            return View();
        }
        [HttpPost]
        public ActionResult BachelorCreate([Bind(Include = "bachelordecorations,bachelortheme,bachelorchairs,bachelortables,bachelorhallcapacity,bachelordate,bachelortime,bachelorcakes,bachelorlocation,bacheloreventcost,bachelorbeverages")] BachelorParty bat)
        {
            if (ModelState.IsValid)
            {

                int lastUserId = EventManagementEntities.BachelorParties.Any() ? EventManagementEntities.BachelorParties.Max(u => u.id) : 0;

                int? userId = TempData["UserId"] as int?;

                int? bateventId = TempData["eventid"] as int?;
                if (userId.HasValue && bateventId.HasValue)
                {
                    bat.bacheloruserid = userId.Value;
                    bat.bachelorid = bateventId.Value;

                }

                bat.id = lastUserId + 1;
                bat.bachelorhallcapacity = 500;
                bat.bacheloreventcost = 1000;

                EventManagementEntities.BachelorParties.Add(bat);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("BachelorDetails", new { id = bat.id });
            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult BachelorDetails(int? id)
        {
            try
            {
                BachelorParty selectedevent = (from s in EventManagementEntities.BachelorParties where s.id == id select s).FirstOrDefault();
                return View(selectedevent);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpStatusCodeResult(404, "Error in eventnameid" + ex.Message);
            }

        }
    }
}
