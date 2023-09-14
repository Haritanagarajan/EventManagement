using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//[assembly: CLSCompliant(true)]

namespace EventManagement.Controllers
{
    public class ReunionController : Controller
    {
        // GET: Reunion
        EventManagement2Entities1 EventManagementEntities = new EventManagement2Entities1();


        // GET: Booking
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult ReunionCreate()
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
        public ActionResult ReunionCreate([Bind(Include = "reuniondecorations,reuniontheme,reunionchairs,reuniontables,reunionhallcapacity,reuniondate,reuniontime,reunioncakes,reunionlocation,reunioneventcost,reunionbeverages,reunionPhotography,reunionStyling,reunionHospitality")] Reunion re)
        {
            if (ModelState.IsValid)
            {

                int lastUserId = EventManagementEntities.Reunions.Any() ? EventManagementEntities.Reunions.Max(u => u.id) : 0;

                int? userId = TempData["UserId"] as int?;

                int? reeventId = TempData["eventid"] as int?;

                if (userId.HasValue && reeventId.HasValue)
                {
                    re.reunionuserid = userId.Value;
                    re.reunionid = reeventId.Value;

                }

                re.id = lastUserId + 1;
                re.reunionhallcapacity = 500;
                re.reunioneventcost = 1000;

                EventManagementEntities.Reunions.Add(re);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("ReunionDetails", new { id = re.id });
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            Reunion re = EventManagementEntities.Reunions.Find(id);

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

            return View(re);
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,reunionid,reunionuserid,reuniondecorations,reuniontheme,reunionchairs,reuniontables,reunionhallcapacity,reuniondate,reuniontime,reunioncakes,reunionlocation,reunioneventcost,reunionbeverages,reunionPhotography,reunionStyling,reunionHospitality")] Reunion re)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int? userId = TempData["UserId"] as int?;

                    int? reventId = TempData["eventid"] as int?;
                    if (userId.HasValue && reventId.HasValue)
                    {
                        re.reunionuserid = userId.Value;
                        re.reunionid = reventId.Value;

                    }
                    EventManagementEntities.Entry(re).State = EntityState.Modified;
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
        public ActionResult ReunionDetails(int? id)
        {
            try
            {
                Reunion selectedevent = (from s in EventManagementEntities.Reunions where s.id == id select s).FirstOrDefault();
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
