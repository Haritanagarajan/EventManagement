using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Razorpay.Api;
using System.Web.UI;
using PagedList;
using PagedList.Mvc;
using EventManagement.Utility;

namespace EventManagement.Controllers
{
    public class CakeController : Controller
    {

        // GET: Cakes
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();

        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page)
        {
            List<caketable> caketable = EventManagementEntities.caketables.ToList();
            //int pageSize = 10;
            //int pageNumber = (page ?? 1);
            return View(caketable.ToPagedList(page ?? 1,3));
        }

        [Authorize(Roles = "User")]
        public ActionResult Index1(string search)
        {
           

            List<caketable> caketable = EventManagementEntities.caketables.ToList();

            return View(EventManagementEntities.caketables.Where(x=>x.cakesavailable.StartsWith(search)|| search == null).ToList());
        }


        [Authorize(Roles = "Admin")]

        [HttpGet]
        public ActionResult CakeCreate(int? id)
        {
            caketable caketable = EventManagementEntities.caketables.Find(id);
            return View(caketable);
        }

        [HttpPost]
        public ActionResult CakeCreate(HttpPostedFileBase cakeimage, [Bind(Include = "cakesavailable")] caketable caketables)
        {
            if (ModelState.IsValid)
            {
                byte[] profile;

                using (var reader = new BinaryReader(cakeimage.InputStream))
                {
                    profile = reader.ReadBytes(cakeimage.ContentLength);
                }
                caketables.cakeimage = profile;

                int lastUserId = EventManagementEntities.caketables.Max(u => u.cakeid);

                caketables.cakeid = lastUserId + 1;
                EventManagementEntities.caketables.Add(caketables);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new CustomDeleteException("Invalid ID. The ID cannot be null.");
                }

                caketable caketable = EventManagementEntities.caketables.Find(id);

                if (caketable == null)
                {
                    throw new CustomDeleteException($"Cake Table with ID {id} not found.");
                }

                return View(caketable);
            }
            catch (CustomDeleteException ex)
            {
                
                return RedirectToAction("Error404", "Error", new { message = ex.Message });
            }
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult LocationDeleteConfirmed(int? id)
        {
            caketable tr = EventManagementEntities.caketables.Find(id);
            EventManagementEntities.caketables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            caketable caketable = EventManagementEntities.caketables.Find(id);
            return View(caketable);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            caketable caketable = EventManagementEntities.caketables.Find(id);
            return View(caketable);
        }

        [HttpPost]
        public ActionResult Edit(int? id, HttpPostedFileBase cakeimage, [Bind(Include = "cakesavailable,caked")] caketable updatecake)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            caketable existingUser = EventManagementEntities.caketables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (cakeimage != null && cakeimage.ContentLength > 0)
                {
                    byte[] profile;

                    using (var reader = new BinaryReader(cakeimage.InputStream))
                    {
                        profile = reader.ReadBytes(cakeimage.ContentLength);
                    }

                    existingUser.cakeimage = profile;
                }

                existingUser.cakesavailable = updatecake.cakesavailable;
                existingUser.caked = updatecake.caked;



                EventManagementEntities.Entry(existingUser).State = EntityState.Modified;

                try
                {
                    EventManagementEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                }
            }

            return View(existingUser);
        }
    }
}