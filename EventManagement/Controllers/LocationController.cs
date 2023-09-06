using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    

    public class LocationController : Controller
    {

        // GET: location
        EventManagementEntities3 EventManagementEntities3 = new EventManagementEntities3();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            List<locationtable> locations = EventManagementEntities3.locationtables.ToList();
            return View(locations);
        }
        // GET: Location
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public ActionResult LocationCreate(int? id)
        {
            locationtable locations = EventManagementEntities3.locationtables.Find(id);
            return View(locations);
        }

        [HttpPost]
        public ActionResult LocationCreate([Bind(Include = "locationid,locationname,pincode")] locationtable locations)
        {
            if (ModelState.IsValid)
            {
                EventManagementEntities3.locationtables.Add(locations);
                EventManagementEntities3.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}