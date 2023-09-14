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
    public class BirthdayController : Controller
    {
        EventManagement2Entities EventManagementEntities = new EventManagement2Entities();

       
        // GET: Booking
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult BirthdayCreate()
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
        public ActionResult BirthdayCreate([Bind(Include = "bdaydecorations,bdaytheme,bdaychairs,bdaytables,bdayhallcapacity,bdaydate,bdaytime,bdaycakes,bdaylocation,bdayeventcost,bdaybeverages")] birthdaytable bday)
        {
            if (ModelState.IsValid)
            {
                
                int lastUserId = EventManagementEntities.birthdaytables.Any() ? EventManagementEntities.birthdaytables.Max(u => u.id) : 0;
                
                int? userId = TempData["UserId"] as int?;

                int? bdayeventId = TempData["eventid"] as int?;
                if (userId.HasValue && bdayeventId.HasValue)
                {
                    bday.bdayuserid = userId.Value;
                    bday.bdayid = bdayeventId.Value;

                }

                bday.id = lastUserId + 1;
                bday.bdayhallcapacity = 500;
                bday.bdayeventcost = 1000;

                EventManagementEntities.birthdaytables.Add(bday);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("BirthdayDetails", new { id = bday.id });
            }
            return View();
        }




        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            birthdaytable bday = EventManagementEntities.birthdaytables.Find(id);

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

            return View(bday);
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "id,bdayid,bdayuserid,bdaydecorations,bdaytheme,bdaychairs,bdaytables,bdayhallcapacity,bdaydate,bdaytime,bdaycakes,bdaylocation,bdayeventcost,bdaybeverages")] birthdaytable bday)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int? userId = TempData["UserId"] as int?;

                    int? bdayventId = TempData["eventid"] as int?;
                    if (userId.HasValue && bdayventId.HasValue)
                    {
                        bday.bdayuserid = userId.Value;
                        bday.bdayid = bdayventId.Value;

                    }
                    EventManagementEntities.Entry(bday).State = EntityState.Modified;
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
        public ActionResult BirthdayDetails(int? id)
        {
            try
            {
                birthdaytable selectedevent = (from s in EventManagementEntities.birthdaytables where s.id == id select s).FirstOrDefault();
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



//    id int primary key,
//   bdayuserid int references Usertable(TUserid),
//    bdayid int references EventNames(eventid),
//bdaydecorations int references decorationtable(decorid),
//bdaytheme int references themetable(themeid),
//bdaychairs int,
//bdaytables int,
//bdayhallcapacity int,
//bdaydate int references datetable(dateid),
//bdaytime int references timetable(timeid),
//bdaycakes int references caketable(cakeid),
//bdaylocation int references locationtable(locationid),
//bdayeventcost bigint,
//bdaybeverages bit not null default 0