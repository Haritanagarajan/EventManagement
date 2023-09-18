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
    public class CocktailController : Controller
    {
        // GET: Cocktail
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();


        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult CocktailCreate()
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
        public ActionResult CocktailCreate([Bind(Include = "cockdatetime,cockdecorations,cocktheme,cockchairs,cocktables,cockhallcapacity,cockcakes,cocklocation,cockeventcost,cockbeverages")] CocktailParty cock, DateTime cockdatetime)
        {
            if (ModelState.IsValid)
            {
                List<CocktailParty> bookedcocktail = EventManagementEntities.CocktailParties.ToList();
                bool isInvalid = false;

                foreach (var item in bookedcocktail)
                {

                    DateTime foundTime = Convert.ToDateTime(item.cockdatetime);
                    DateTime enteredTime = cockdatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }


                }

                if (isInvalid)
                {
                    ModelState.AddModelError("cockdatetime", "The selected datetime is too close to an existing cocktail party.");

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

                    int lastUserId = EventManagementEntities.CocktailParties.Any() ? EventManagementEntities.CocktailParties.Max(u => u.id) : 0;

                    int? userId = Session["UserId"] as int?;

                    int? cockeventId = Session["eventid"] as int?;
                    if (userId.HasValue && cockeventId.HasValue)
                    {
                        cock.cockuserid = userId.Value;
                        cock.cockid = cockeventId.Value;

                    }

                    cock.id = lastUserId + 1;
                    cock.cockhallcapacity = 500;
                    cock.cockeventcost = 1000;

                    EventManagementEntities.CocktailParties.Add(cock);
                    EventManagementEntities.SaveChanges();
                    return RedirectToAction("CocktailDetails", new { id = cock.id });

                }
            }
            return View();
        }


        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            CocktailParty baby = EventManagementEntities.CocktailParties.Find(id);

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
        public ActionResult Edit([Bind(Include = "cockdatetime,id,cockuserid,cockid,cockdecorations,cocktheme,cockchairs,cocktables,cockhallcapacity,cockcakes,cocklocation,cockeventcost,cockbeverages")] CocktailParty cock, DateTime cockdatetime)
        {
            if (ModelState.IsValid)
            {
                List<CocktailParty> bookedCocktailParties = EventManagementEntities.CocktailParties.Where(c => c.id != cock.id).ToList(); // Exclude the current cocktail party being edited from the check
                bool isInvalid = false;

                foreach (var item in bookedCocktailParties)
                {
                    DateTime foundTime = Convert.ToDateTime(item.cockdatetime);
                    DateTime enteredTime = cockdatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }
                }

                if (isInvalid)
                {
                    ModelState.AddModelError("cockdatetime", "The selected datetime is too close to an existing cocktail party.");

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

                    return View(cock);
                }
                else
                {
                    try
                    {
                        int? userId = Session["UserId"] as int?;
                        int? cockventId = Session["eventid"] as int?;

                        if (userId.HasValue && cockventId.HasValue)
                        {
                            cock.cockuserid = userId.Value;
                            cock.cockid = cockventId.Value;
                        }

                        EventManagementEntities.Entry(cock).State = EntityState.Modified;
                        EventManagementEntities.SaveChanges();
                        return Content("Successfully edited");
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                    }
                }
            }

         
            return View(cock);
        }




        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult CocktailDetails(int? id)
        {
            try
            {
                CocktailParty selectedevent = (from s in EventManagementEntities.CocktailParties where s.id == id select s).FirstOrDefault();
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
            CocktailParty cock = EventManagementEntities.CocktailParties.Find(id);
            return View(cock);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int? id)
        {
            CocktailParty cock = EventManagementEntities.CocktailParties.Find(id);
            EventManagementEntities.CocktailParties.Remove(cock);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index1", "Cart");
        }


    }
}
