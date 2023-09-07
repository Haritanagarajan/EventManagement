using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EventManagement.Controllers
{
    //database EventManagementEntities2
    //usermodel  RoleTable      //RoleTables
    //rolemodel  UserTable      //UserTables

    [Authorize]
    [AllowAnonymous]

    public class AuthendicationController : Controller
    {

        EventManagementEntities4 EventManagementEntities = new EventManagementEntities4();
        // GET: Authendication
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [Authorize(Roles="Admin")]

        [HttpGet]
        public ActionResult Index()
        {
            List<UserTable> users = EventManagementEntities.UserTables.ToList();
            return View(users);
        }

        [HttpGet]
        public ActionResult Register()
        {
            List<UserTable> role = EventManagementEntities.UserTables.ToList();
            ViewBag.specificroles = new SelectList(role, "TRoleid", "TRolename");
            return View();
        }

        [HttpPost]
        public ActionResult Register(HttpPostedFileBase Tprofile ,[Bind(Include = "TUsername,TPassword,TEmail,TMobile")] UserTable user)
        {
            if (ModelState.IsValid)
            {

                byte[] profile;

                using (var reader = new BinaryReader(Tprofile.InputStream))
                {
                    profile = reader.ReadBytes(Tprofile.ContentLength);
                }
                user.Tprofile = profile;

                int lastUserId = EventManagementEntities.UserTables.Max(u => u.TUserid);

                user.TUserid = lastUserId + 1;

                DateTime actuallogindate = DateTime.Now;

                user.LastLoginDate = actuallogindate;

                int roleid = 2;

                user.TRoleid = roleid;

                EventManagementEntities.UserTables.Add(user);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserTable user)
        {
            EventManagementEntities4 usertabledatabase = new EventManagementEntities4();

            Validate_User_Result roleUser = usertabledatabase.Validate_User(user.TUsername, user.TPassword).FirstOrDefault();
            string message = string.Empty;
            switch (roleUser.TUserid.Value)
            {
                case -1:
                    message = "Username and/or password is incorrect.";
                    break;
                default:

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.TUsername, DateTime.Now, DateTime.Now.AddMinutes(30), false, roleUser.Roles, FormsAuthentication.FormsCookiePath);
                    string hash = FormsAuthentication.Encrypt(ticket);
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                    if (ticket.IsPersistent)
                    {
                        cookie.Expires = ticket.Expiration;
                    }
                    Response.Cookies.Add(cookie);
                    TempData["UserId"] = roleUser.TUserid.Value;
                    return RedirectToAction("EventsName","Events");
            }

            ViewBag.Message = message;
            return View(user);
        }


        [HttpGet]
        [Authorize(Roles= "Admin")]
        public ActionResult Delete(int? id)
        {
            UserTable user = EventManagementEntities.UserTables.Find(id);
            return View(user); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int? id)
        {
            UserTable tr = EventManagementEntities.UserTables.Find(id);
            EventManagementEntities.UserTables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            UserTable user = EventManagementEntities.UserTables.Find(id);
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int? id)
        {
            UserTable user = EventManagementEntities.UserTables.Find(id);
            List<RoleTable> role = EventManagementEntities.RoleTables.ToList();
            ViewBag.Roles = new SelectList(role, "TRoleid", "TRolename");
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(int? id, HttpPostedFileBase Tprofile, [Bind(Include = "TUsername,TPassword,TEmail,TMobile,TRoleid")] UserTable updatedUser)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserTable existingUser = EventManagementEntities.UserTables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (Tprofile != null && Tprofile.ContentLength > 0)
                {
                    byte[] profile;

                    using (var reader = new BinaryReader(Tprofile.InputStream))
                    {
                        profile = reader.ReadBytes(Tprofile.ContentLength);
                    }

                    existingUser.Tprofile = profile;
                }

                existingUser.TUsername = updatedUser.TUsername;
                existingUser.TPassword = updatedUser.TPassword;
                existingUser.TEmail = updatedUser.TEmail;
                existingUser.TMobile = updatedUser.TMobile;
                existingUser.LastLoginDate = DateTime.Now;
                existingUser.TRoleid = updatedUser.TRoleid;


                EventManagementEntities.Entry(existingUser).State = EntityState.Modified;

                try
                {
                    EventManagementEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                   
                    ModelState.AddModelError(string.Empty, "Concurrency error occurred.");
                }
            }

          
            return View(existingUser);
        }


        public ActionResult LogoutForm()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LoginForm", "Login");
        }


    public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }


}



