using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EventManagement.Controllers
{
    public class ReunionController : Controller
    {
        // GET: Reunion
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();


        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult ReunionCreate()
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
        public ActionResult ReunionCreate([Bind(Include = "reuniondatetime,reuniondecorations,reuniontheme,reunionchairs,reuniontables,reunionhallcapacity,reunioncakes,reunionlocation,reunioneventcost,reunionbeverages,reunionPhotography,reunionStyling,reunionHospitality")] Reunion re,DateTime reuniondatetime)
        {
            if (ModelState.IsValid)
            {
                List<Reunion> bookedreunion = EventManagementEntities.Reunions.ToList();
                bool isInvalid = false;

                foreach (var item in bookedreunion)
                {

                    DateTime foundTime = Convert.ToDateTime(item.reuniondatetime);
                    DateTime enteredTime = reuniondatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }


                }
                if (isInvalid)
                {
                    ModelState.AddModelError("reuniondatetime", "The selected datetime is too close to an existing reunion.");


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
                    int lastUserId = EventManagementEntities.Reunions.Any() ? EventManagementEntities.Reunions.Max(u => u.id) : 0;

                    int? userId = Session["UserId"] as int?;

                    int? reeventId = Session["eventid"] as int?;

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
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            Reunion re = EventManagementEntities.Reunions.Find(id);

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

            return View(re);
        }


        


        [HttpPost]
        public ActionResult Edit([Bind(Include = "reuniondatetime,id,reunionid,reunionuserid,reuniondecorations,reuniontheme,reunionchairs,reuniontables,reunionhallcapacity,reunioncakes,reunionlocation,reunioneventcost,reunionbeverages,reunionPhotography,reunionStyling,reunionHospitality")] Reunion re, DateTime reuniondatetime)
        {
            if (ModelState.IsValid)
            {
                List<Reunion> bookedReunions = EventManagementEntities.Reunions.Where(r => r.id != re.id).ToList(); 
                bool isInvalid = false;

                foreach (var item in bookedReunions)
                {
                    DateTime foundTime = Convert.ToDateTime(item.reuniondatetime);
                    DateTime enteredTime = reuniondatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }
                }

                if (isInvalid)
                {
                    ModelState.AddModelError("reuniondatetime", "The selected datetime is too close to an existing reunion.");

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

                    return View(re);
                }
                else
                {
                    try
                    {
                        int? userId = Session["UserId"] as int?;
                        int? reventId = Session["eventid"] as int?;

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
                        ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                    }
                }
            }


            return View(re);
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

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Delete(int? id)
        {
            Reunion re = EventManagementEntities.Reunions.Find(id);
            return View(re);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int? id)
        {
            Reunion re = EventManagementEntities.Reunions.Find(id);
            EventManagementEntities.Reunions.Remove(re);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index1", "Cart");
        }
    }
}
