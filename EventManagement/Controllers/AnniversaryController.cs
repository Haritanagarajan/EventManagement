using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

[assembly: CLSCompliant(true)]

namespace EventManagement.Controllers
{
    public class AnniversaryController : Controller
    {
        // GET: Anniversary
        EventManagement2Entities EventManagementEntities = new EventManagement2Entities();


        // GET: Booking
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult AnniCreate()
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
        public ActionResult AnniCreate([Bind(Include = "annidecorations,annitheme,annichairs,annitables,annihallcapacity,annidate,annitime,annicakes,annilocation,annieventcost,annibeverages,anniPhotography,anniStyling,anniHospitality")] Anniversary anni)
        {
            if (ModelState.IsValid)
            {

                int lastUserId = EventManagementEntities.Anniversaries.Any() ? EventManagementEntities.Anniversaries.Max(u => u.id) : 0;

                int? userId = TempData["UserId"] as int?;

                int? annieventId = TempData["eventid"] as int?;
                if (userId.HasValue && annieventId.HasValue)
                {
                    anni.anniuserid = userId.Value;
                    anni.anniid = annieventId.Value;

                }

                anni.id = lastUserId + 1;
                anni.annihallcapacity = 500;
                anni.annieventcost = 1000;

                EventManagementEntities.Anniversaries.Add(anni);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("AnniDetails", new { id = anni.id });
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            Anniversary anni = EventManagementEntities.Anniversaries.Find(id);

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

            return View(anni);
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,anniid,anniuserid,annieventcost,annihallcapacity,annidecorations,annitheme,annichairs,annitables,annihallcapacity,annidate,annitime,annicakes,annilocation,annieventcost,annibeverages,anniPhotography,anniStyling,anniHospitality")] Anniversary updatedAnni)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int? userId = TempData["UserId"] as int?;

                    int? annieventId = TempData["eventid"] as int?;
                    if (userId.HasValue && annieventId.HasValue)
                    {
                        updatedAnni.anniuserid = userId.Value;
                        updatedAnni.anniid = annieventId.Value;

                    }
                    EventManagementEntities.Entry(updatedAnni).State = EntityState.Modified;
                    EventManagementEntities.SaveChanges();
                    return Content("Successfully edited");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency conflicts here
                    ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                }

            }

            return View();
        }





        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult AnniDetails(int? id)
        {
            try
            {
                Anniversary selectedevent = (from s in EventManagementEntities.Anniversaries where s.id == id select s).FirstOrDefault();
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
