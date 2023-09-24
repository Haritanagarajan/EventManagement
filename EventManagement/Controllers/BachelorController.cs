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
    public class BachelorController : Controller
    {
        // GET: Bachelor
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();


        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult BachelorCreate()
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
        public ActionResult BachelorCreate([Bind(Include = "bachelordatetime,bachelordecorations,bachelortheme,bachelorchairs,bachelortables,bachelorhallcapacity,bachelorcakes,bachelorlocation,bacheloreventcost,bachelorbeverages")] BachelorParty bat,DateTime bachelordatetime)
        {
            if (ModelState.IsValid)
            {
                List<BachelorParty> bookedbachelor = EventManagementEntities.BachelorParties.ToList();
                bool isInvalid = false;

                foreach (var item in bookedbachelor)
                {

                    DateTime foundTime = Convert.ToDateTime(item.bachelordatetime);
                    DateTime enteredTime = bachelordatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }


                }

                if (isInvalid)
                {
                    ModelState.AddModelError("bachelordatetime", "The selected datetime is too close to an existing bachelor party.");
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

                int lastUserId = EventManagementEntities.BachelorParties.Any() ? EventManagementEntities.BachelorParties.Max(u => u.id) : 0;

                int? userId = Session["UserId"] as int?;

                int? bateventId = Session["eventid"] as int?;
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
            }
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            BachelorParty bachelor = EventManagementEntities.BachelorParties.Find(id);

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

            return View(bachelor);
        }


      

        [HttpPost]
        public ActionResult Edit([Bind(Include = "bachelordatetime,id,bacheloruserid,bachelorid,bachelordecorations,bachelortheme,bachelorchairs,bachelortables,bachelorhallcapacity,bachelorcakes,bachelorlocation,bacheloreventcost,bachelorbeverages")] BachelorParty bat, DateTime bachelordatetime)
        {
            if (ModelState.IsValid)
            {
                List<BachelorParty> bookedBachelorParties = EventManagementEntities.BachelorParties.Where(b => b.id != bat.id).ToList(); 
                bool isInvalid = false;

                foreach (var item in bookedBachelorParties)
                {
                    DateTime foundTime = Convert.ToDateTime(item.bachelordatetime);
                    DateTime enteredTime = bachelordatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }
                }

                if (isInvalid)
                {
                    ModelState.AddModelError("bachelordatetime", "The selected datetime is too close to an existing bachelor party.");

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


                    return View(bat);
                }
                else
                {
                    try
                    {
                        int? userId = Session["UserId"] as int?;
                        int? batventId = Session["eventid"] as int?;

                        if (userId.HasValue && batventId.HasValue)
                        {
                            bat.bacheloruserid = userId.Value;
                            bat.bachelorid = batventId.Value;
                        }

                        EventManagementEntities.Entry(bat).State = EntityState.Modified;
                        EventManagementEntities.SaveChanges();
                        return Content("Successfully edited");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                    }
                }
            }

  
            return View(bat);
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

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new CustomDeleteException("Invalid ID. The ID cannot be null.");
            }

            BachelorParty batch = EventManagementEntities.BachelorParties.Find(id);

            if (batch == null)
            {
                throw new CustomDeleteException($"Bachelor Party with ID {id} not found.");
            }

            return View(batch);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int? id)
        {
            BachelorParty batch = EventManagementEntities.BachelorParties.Find(id);
            EventManagementEntities.BachelorParties.Remove(batch);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index1", "Cart");
        }
    }
}
