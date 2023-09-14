using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    public class WeddingController : Controller
    {
        // GET: Wedding
        EventManagement2Entities1 EventManagementEntities = new EventManagement2Entities1();


        // GET: Booking
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult WeddingCreate()
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
        public ActionResult WeddingCreate([Bind(Include = "weddingdecorations,weddingtheme,weddingchairs,weddingtables,weddinghallcapacity,weddingdate,weddingtime,weddingcakes,weddinglocation,weddingeventcost,weddingbeverages,weddingPhotography,weddingStyling,weddingHospitality")] Wedding wed)
        {
            if (ModelState.IsValid)
            {

                int lastUserId = EventManagementEntities.Weddings.Any() ? EventManagementEntities.Weddings.Max(u => u.id) : 0;

                int? userId = TempData["UserId"] as int?;

                int? wedeventId = TempData["eventid"] as int?;
                if (userId.HasValue && wedeventId.HasValue)
                {
                    wed.weddinguserid = userId.Value;
                    wed.weddingid = wedeventId.Value;

                }

                wed.id = lastUserId + 1;
                wed.weddinghallcapacity = 500;
                wed.weddingeventcost = 1000;

                EventManagementEntities.Weddings.Add(wed);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("WeddingDetails", new { id = wed.id });
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            Wedding baby = EventManagementEntities.Weddings.Find(id);

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

            return View(baby);
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,weddingid,weddinguserid,weddingdecorations,weddingtheme,weddingchairs,weddingtables,weddinghallcapacity,weddingdate,weddingtime,weddingcakes,weddinglocation,weddingeventcost,weddingbeverages,weddingPhotography,weddingStyling,weddingHospitality")] Wedding wed)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int? userId = TempData["UserId"] as int?;

                    int? wedventId = TempData["eventid"] as int?;
                    if (userId.HasValue && wedventId.HasValue)
                    {
                        wed.weddinguserid = userId.Value;
                        wed.weddingid = wedventId.Value;

                    }
                    EventManagementEntities.Entry(wed).State = EntityState.Modified;
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
        public ActionResult WeddingDetails(int? id)
        {
            try
            {
                Wedding selectedevent = (from s in EventManagementEntities.Weddings where s.id == id select s).FirstOrDefault();
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
