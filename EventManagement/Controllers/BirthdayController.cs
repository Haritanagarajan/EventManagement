﻿using EventManagement.Models;
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
    public class BirthdayController : Controller
    {
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();

       
        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult BirthdayCreate()
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
        public ActionResult BirthdayCreate([Bind(Include = "bdaydatetime,bdaydecorations,bdaytheme,bdaychairs,bdaytables,bdayhallcapacity,bdaycakes,bdaylocation,bdayeventcost,bdaybeverages")] birthdaytable bday,DateTime bdaydatetime)
        {
            if (ModelState.IsValid)
            {
                List<birthdaytable> bookedbday = EventManagementEntities.birthdaytables.ToList();
                bool isInvalid = false;

                foreach (var item in bookedbday)
                {

                    DateTime foundTime = Convert.ToDateTime(item.bdaydatetime);
                    DateTime enteredTime = bdaydatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }


                }

                if (isInvalid)
                {

                    ModelState.AddModelError("bdaydatetime", "The selected datetime is too close to an existing birthday event.");

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

                int lastUserId = EventManagementEntities.birthdaytables.Any() ? EventManagementEntities.birthdaytables.Max(u => u.id) : 0;
                
                int? userId = Session["UserId"] as int?;

                int? bdayeventId = Session["eventid"] as int?;
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
            }
            return View();
        }




        [HttpGet]
        [Authorize(Roles = "User")]

        public ActionResult Edit(int? id)
        {
            birthdaytable bday = EventManagementEntities.birthdaytables.Find(id);

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

            return View(bday);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "bdaydatetime,id,bdayid,bdayuserid,bdaydecorations,bdaytheme,bdaychairs,bdaytables,bdayhallcapacity,bdaycakes,bdaylocation,bdayeventcost,bdaybeverages")] birthdaytable bday, DateTime bdaydatetime)
        {
            if (ModelState.IsValid)
            {
                List<birthdaytable> bookedBirthdays = EventManagementEntities.birthdaytables.Where(b => b.id != bday.id).ToList(); 
                bool isInvalid = false;

                foreach (var item in bookedBirthdays)
                {
                    DateTime foundTime = Convert.ToDateTime(item.bdaydatetime);
                    DateTime enteredTime = bdaydatetime;

                    TimeSpan timeDifference = foundTime - enteredTime;

                    if (Math.Abs(timeDifference.TotalHours) < 5)
                    {
                        isInvalid = true;
                        break;
                    }
                }

                if (isInvalid)
                {
                    ModelState.AddModelError("bdaydatetime", "The selected datetime is too close to an existing birthday event.");

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

                    return View(bday);
                }
                else
                {
                    try
                    {
                        int? userId = Session["UserId"] as int?;
                        int? bdayventId = Session["eventid"] as int?;

                        if (userId.HasValue && bdayventId.HasValue)
                        {
                            bday.bdayuserid = userId.Value;
                            bday.bdayid = bdayventId.Value;
                        }

                        EventManagementEntities.Entry(bday).State = EntityState.Modified;
                        EventManagementEntities.SaveChanges();
                        return Content("Successfully edited");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                    }
                }
            }

         

            return View(bday);
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

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new CustomDeleteException("Invalid ID. The ID cannot be null.");
            }

            birthdaytable bday = EventManagementEntities.birthdaytables.Find(id);

            if (bday == null)
            {
                throw new CustomDeleteException($"Birthday Table with ID {id} not found.");
            }

            return View(bday);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int? id)
        {
            birthdaytable bday = EventManagementEntities.birthdaytables.Find(id);
            EventManagementEntities.birthdaytables.Remove(bday);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index1", "Cart");
        }
    }

}

