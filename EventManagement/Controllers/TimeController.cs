using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
   

    public class TimeController : Controller
    {
        // GET: Time
        EventManagementEntities3 EventManagementEntities3 = new EventManagementEntities3();

        [Authorize(Roles = "Admin")]

        public ActionResult Index()
        {
            List<timetable> times = EventManagementEntities3.timetables.ToList();
            return View(times);
        }
        // GET: Date
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public ActionResult TimeCreate(int? id)
        {
            timetable times = EventManagementEntities3.timetables.Find(id);
            return View(times);
        }

        [HttpPost]
        public ActionResult TimeCreate([Bind(Include = "timeid,timesavailable")] timetable timess)
        {
            if (ModelState.IsValid)
            {
                EventManagementEntities3.timetables.Add(timess);
                EventManagementEntities3.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

    }
}