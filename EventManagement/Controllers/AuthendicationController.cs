using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
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

        EventManagementEntities3 EventManagementEntities = new EventManagementEntities3();
         // GET: Authendication
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            List<RoleTable> role = EventManagementEntities.RoleTables.ToList();
            ViewBag.specificroles = new SelectList(role, "TRoleid", "TRolename");
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "TUsername,TPassword,TEmail,TMobile")] UserTable user)
        {
            if (ModelState.IsValid)
            {
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
            EventManagementEntities3 usertabledatabase = new EventManagementEntities3();

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

      
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }


    }
}