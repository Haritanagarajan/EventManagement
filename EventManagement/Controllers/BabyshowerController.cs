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
    public class BabyshowerController : Controller
    {
        // GET: BabySHOWER
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();


        // GET: Booking
        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult BabyshowerCreate()
        {

          

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
        public ActionResult BabyshowerCreate([Bind(Include = "babyshowerdatetime,babyshowerdecorations,babyshowertheme,babyshowerchairs,babyshowertables,babyshowerhallcapacity,babyshowercakes,babyshowerlocation,babyshowereventcost,babyshowerbeverages")] babyshowertable baby)
        {
            if (ModelState.IsValid)
            {
                int lastUserId = EventManagementEntities.babyshowertables.Any() ? EventManagementEntities.babyshowertables.Max(u => u.id) : 0;
                int? userId = Session["UserId"] as int?;
                int? babyeventId = Session["eventid"] as int?;
                if (userId.HasValue && babyeventId.HasValue)
                {
                    baby.babyshoweruserid = userId.Value;
                    baby.babyshowerid = babyeventId.Value;

                }

                baby.id = lastUserId + 1;
                baby.babyshowerhallcapacity = 500;
                baby.babyshowereventcost = 1000;

                EventManagementEntities.babyshowertables.Add(baby);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("BabyshowerDetails", new { id = baby.id });
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            babyshowertable baby = EventManagementEntities.babyshowertables.Find(id);

           

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
        public ActionResult Edit([Bind(Include = "babyshowerdatetime,id,babyshoweruserid,babyshowerid,babyshowerdecorations,babyshowertheme,babyshowerchairs,babyshowertables,babyshowerhallcapacity,babyshowercakes,babyshowerlocation,babyshowereventcost,babyshowerbeverages")] babyshowertable baby)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int? userId = Session["UserId"] as int?;

                    int? babyventId = Session["eventid"] as int?;
                    if (userId.HasValue && babyventId.HasValue)
                    {
                        baby.babyshoweruserid = userId.Value;
                        baby.babyshowerid = babyventId.Value;

                    }
                    EventManagementEntities.Entry(baby).State = EntityState.Modified;
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
        public ActionResult BabyshowerDetails(int? id)
        {

            try
            {
                babyshowertable selectedevent = (from s in EventManagementEntities.babyshowertables where s.id == id select s).FirstOrDefault();
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



//id int primary key,
//babyshoweruserid int references Usertable(TUserid),
//babyshowerid int references EventNames(eventid),
//babyshowerdecorations int references decorationtable(decorid),
//babyshowertheme int references themetable(themeid),
//babyshowerchairs int,
//babyshowertables int,
//babyshowerhallcapacity int,
//babyshowerdate int references datetable(dateid),
//babyshowertime int references timetable(timeid),
//babyshowercakes int references caketable(cakeid),
//babyshowerlocation  int references locationtable(locationid),
//babyshowereventcost bigint,
//babyshowerbeverages bit not null default 0