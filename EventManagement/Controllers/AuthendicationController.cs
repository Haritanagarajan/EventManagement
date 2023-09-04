using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EventManagement.Controllers
{
    //database EventManagementEntities
    //usermodel  RoleTable      //RoleTables
    //rolemodel  UserTable      //UserTables

    [Authorize]

    public class AuthendicationController : Controller
    {

        EventManagementEntities EventManagementEntities = new EventManagementEntities();
         // GET: Authendication
         [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            List<UserTable> user = EventManagementEntities.UserTables.ToList();
            ViewBag.UserTables = new SelectList(user, "TRoleid", "TRolename");
            return View();
        }

        [HttpPost]
        public ActionResult Register([Bind(Include = "TUsername,TEmail,TMobile")] UserTable user)
        {
            if (ModelState.IsValid)

            {
                EventManagementEntities.UserTables.Add(user);
                EventManagementEntities.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserTable user)
        {
            EventManagementEntities usertabledatabase = new EventManagementEntities();

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
                    return RedirectToAction("AuthNav");
            }

            ViewBag.Message = message;
            return View(user);
        }

        [AllowAnonymous]
        [Authorize]
        public ActionResult AuthNav()
        {
            return View();
        }

        public ActionResult UserDetails()
        {
            EventManagementEntities usertabledatabase = new EventManagementEntities();
            List<UserTable> users = usertabledatabase.UserTables.ToList();
            return View(users);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}