using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
   
    public class DateController : Controller
    {
        EventManagementEntities4 EventManagementEntities3 = new EventManagementEntities4();

        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            List<datetable> dates = EventManagementEntities3.datetables.ToList();
            return View(dates);
        }
        // GET: Date
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public ActionResult DateCreate(int? id)
        {
            datetable dates = EventManagementEntities3.datetables.Find(id);
            return View(dates);
        }

        [HttpPost]
        public ActionResult DateCreate([Bind(Include = "dateid,datesavailable")] datetable dates)
        {
            if(ModelState.IsValid)
            {
                EventManagementEntities3.datetables.Add(dates);
                EventManagementEntities3.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


    }
}