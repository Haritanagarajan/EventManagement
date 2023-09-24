using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventManagement.Utility;

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
            List<decorationtable> decorationOptions = EventManagementEntities.decorationtables.ToList();
            ViewBag.DecorationOptions = decorationOptions;

            List<themetable> themeOptions = EventManagementEntities.themetables.ToList();
            ViewBag.ThemeOptions = themeOptions;

            List<locationtable> locationOptions = EventManagementEntities.locationtables.ToList();
            ViewBag.LocationOptions = locationOptions;

            List<caketable> cakeOptions = EventManagementEntities.caketables.ToList();
            ViewBag.CakeOptions = cakeOptions;


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
        public ActionResult BabyshowerCreate([Bind(Include = "babyshowerdatetime,babyshowerdecorations,babyshowertheme,babyshowerchairs,babyshowertables,babyshowerhallcapacity,babyshowercakes,babyshowerlocation,babyshowereventcost,babyshowerbeverages")] babyshowertable baby,DateTime babyshowerdatetime)
        {
            if (ModelState.IsValid)
            {
                List<babyshowertable> bookedbaby = EventManagementEntities.babyshowertables.ToList();
                bool isInvalid = false;

                foreach (var item in bookedbaby)
                {

                    DateTime foundTime = Convert.ToDateTime(item.babyshowerdatetime);
                    DateTime enteredTime = babyshowerdatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }

                }

                if (isInvalid)
                {
                    ModelState.AddModelError("babyshowerdatetime", "The selected datetime is too close to an existing baby shower.");

                    List<decorationtable> decorationOptions = EventManagementEntities.decorationtables.ToList();
                    ViewBag.DecorationOptions = decorationOptions;

                    List<themetable> themeOptions = EventManagementEntities.themetables.ToList();
                    ViewBag.ThemeOptions = themeOptions;

                    List<locationtable> locationOptions = EventManagementEntities.locationtables.ToList();
                    ViewBag.LocationOptions = locationOptions;

                    List<caketable> cakeOptions = EventManagementEntities.caketables.ToList();
                    ViewBag.CakeOptions = cakeOptions;

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

                else
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
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            babyshowertable baby = EventManagementEntities.babyshowertables.Find(id);

            List<decorationtable> decorationOptions = EventManagementEntities.decorationtables.ToList();
            ViewBag.DecorationOptions = decorationOptions;

            List<themetable> themeOptions = EventManagementEntities.themetables.ToList();
            ViewBag.ThemeOptions = themeOptions;

            List<locationtable> locationOptions = EventManagementEntities.locationtables.ToList();
            ViewBag.LocationOptions = locationOptions;

            List<caketable> cakeOptions = EventManagementEntities.caketables.ToList();
            ViewBag.CakeOptions = cakeOptions;

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
        public ActionResult Edit([Bind(Include = "babyshowerdatetime,id,babyshoweruserid,babyshowerid,babyshowerdecorations,babyshowertheme,babyshowerchairs,babyshowertables,babyshowerhallcapacity,babyshowercakes,babyshowerlocation,babyshowereventcost,babyshowerbeverages")] babyshowertable baby, DateTime babyshowerdatetime)
        {
            if (ModelState.IsValid)
            {
                List<babyshowertable> bookedBabyShowers = EventManagementEntities.babyshowertables.Where(b => b.id != baby.id).ToList(); 
                bool isInvalid = false;

                foreach (var item in bookedBabyShowers)
                {
                    DateTime foundTime = Convert.ToDateTime(item.babyshowerdatetime);
                    DateTime enteredTime = babyshowerdatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }
                }

                if (isInvalid)
                {
                    ModelState.AddModelError("babyshowerdatetime", "The selected datetime is too close to an existing baby shower.");

                    List<decorationtable> decorationOptions = EventManagementEntities.decorationtables.ToList();
                    ViewBag.DecorationOptions = decorationOptions;

                    List<themetable> themeOptions = EventManagementEntities.themetables.ToList();
                    ViewBag.ThemeOptions = themeOptions;

                    List<locationtable> locationOptions = EventManagementEntities.locationtables.ToList();
                    ViewBag.LocationOptions = locationOptions;

                    List<caketable> cakeOptions = EventManagementEntities.caketables.ToList();
                    ViewBag.CakeOptions = cakeOptions;

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
                else
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
                    catch (DbUpdateConcurrencyException)
                    {
                        ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                    }
                }
            }

    
            return View(baby);
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

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new CustomDeleteException("Invalid ID. The ID cannot be null.");
            }

            babyshowertable baby = EventManagementEntities.babyshowertables.Find(id);

            if (baby == null)
            {
                throw new CustomDeleteException($"Baby Shower with ID {id} not found.");
            }

            return View(baby);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int? id)
        {
            babyshowertable baby = EventManagementEntities.babyshowertables.Find(id);
            EventManagementEntities.babyshowertables.Remove(baby);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index1", "Cart");
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