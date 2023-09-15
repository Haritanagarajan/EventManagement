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
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();


        // GET: Booking
        //[HttpGet]
        //[Authorize(Roles = "User")]
        public ActionResult WeddingCreate()
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
        public ActionResult WeddingCreate([Bind(Include = "weddingdatetime,weddingdecorations,weddingtheme,weddingchairs,weddingtables,weddinghallcapacity,weddingcakes,weddinglocation,weddingeventcost,weddingbeverages,weddingPhotography,weddingStyling,weddingHospitality")] Wedding wed,DateTime weddingtime)
        {
            //if (ModelState.IsValid)
            //{
            List<Wedding> bookedweddings = EventManagementEntities.Weddings.Where(w => w.weddingtime.ToString() == weddingtime.TimeOfDay.ToString()).ToList();
            if (bookedweddings.Count > 0)
            {
                Response.Write("That time slot is unavailable");
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
                int lastUserId = EventManagementEntities.Weddings.Any() ? EventManagementEntities.Weddings.Max(u => u.id) : 0;

                int? userId = Session["UserId"] as int?;

                int? wedeventId = Session["eventid"] as int?;
                if (userId.HasValue && wedeventId.HasValue)
                {
                    wed.weddinguserid = userId.Value;
                    wed.weddingid = wedeventId.Value;

                }

                wed.id = lastUserId + 1;
                wed.weddinghallcapacity = 500;
                wed.weddingeventcost = 1000;
                wed.weddingtime = weddingtime.TimeOfDay;



                EventManagementEntities.Weddings.Add(wed);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("WeddingDetails", new { id = wed.id });
            }
           
            //}
        
        }

        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            Wedding baby = EventManagementEntities.Weddings.Find(id);

            

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
        public ActionResult Edit([Bind(Include = "weddingdatetime,weddingtime,id,weddingid,weddinguserid,weddingdecorations,weddingtheme,weddingchairs,weddingtables,weddinghallcapacity,weddingcakes,weddinglocation,weddingeventcost,weddingbeverages,weddingPhotography,weddingStyling,weddingHospitality")] Wedding wed)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    int? userId = Session["UserId"] as int?;

                    int? wedventId = Session["eventid"] as int?;
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
