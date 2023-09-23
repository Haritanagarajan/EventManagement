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
using CaptchaMvc.HtmlHelpers;


namespace EventManagement.Controllers
{
    //database EventManagement1Entities1

    [Authorize]
    [AllowAnonymous]

    public class AuthendicationController : Controller
    {

        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();
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
            List<Usertable> users = EventManagementEntities.Usertables.ToList();
            return View(users);
        }

        [HttpGet]
        public ActionResult Register()
        {
            List<Usertable> role = EventManagementEntities.Usertables.ToList();
            ViewBag.specificroles = new SelectList(role, "TRoleid", "TRolename");
            return View();
        }

        [HttpPost]
        public ActionResult Register(HttpPostedFileBase TProfile, [Bind(Include = "TUsername,TDob,TPassword,TConfirmPassword,TEmail,TMobile,TAge,TGender")] Usertable user)
        {
            if (ModelState.IsValid)
            {


                if (!this.IsCaptchaValid("Captcha is not valid"))
                {
                    ViewBag.ErrorMessage = "Error : Captcha code is not Valid";
                    return View("Register");
                }

                if(TProfile != null)
                {

                    ViewBag.ErrorMessage = "Error : Profile is required";

                    byte[] profile;


                    using (var reader = new BinaryReader(TProfile.InputStream))
                    {
                        profile = reader.ReadBytes(TProfile.ContentLength);
                    }
                    user.TProfile = profile;

                    int lastUserId = EventManagementEntities.Usertables.Max(u => u.TUserid);

                    user.TUserid = lastUserId + 1;

                    DateTime actuallogindate = DateTime.Now;

                    user.LastLoginDate = actuallogindate;

                    int roleid = 2;

                    user.TRoleid = roleid;

                    Session["UserMobile"] = user.TMobile.Value;
                    Session["UserEmail"] = user.TEmail;


                    EventManagementEntities.Usertables.Add(user);
                    EventManagementEntities.SaveChanges();
                }
               
                return RedirectToAction("Login");
            }


            

            return View(user);
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Usertable user)
        {
            EventManagement2Entities2 usertabledatabase = new EventManagement2Entities2();

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
                    Session["UserId"] = roleUser.TUserid.Value;
                    Usertable userDetails = EventManagementEntities.Usertables.Where(u=>u.TUserid==roleUser.TUserid.Value).FirstOrDefault();
                    Session["UserEmail"] = userDetails.TEmail;
                    Session["UserContact"] = userDetails.TMobile;
                    return RedirectToAction("EventsName","Events");
            }

            ViewBag.Message = message;
            return View(user);
        }


        [HttpGet]
        [Authorize(Roles= "Admin")]
        public ActionResult Delete(int? id)
        {
            Usertable user = EventManagementEntities.Usertables.Find(id);
            return View(user); 
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserDeleteConfirmed(int? id)
        {
            Usertable tr = EventManagementEntities.Usertables.Find(id);
            EventManagementEntities.Usertables.Remove(tr);
            EventManagementEntities.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public ActionResult Details(int? id)
        {
            Usertable user = EventManagementEntities.Usertables.Find(id);
            return View(user);
        }

        [HttpGet]
        [Authorize(Roles="Admin")]
        public ActionResult Edit(int? id)
        {
            Usertable user = EventManagementEntities.Usertables.Find(id);
            List<Roletable> role = EventManagementEntities.Roletables.ToList();
            ViewBag.Roles = new SelectList(role, "TRoleid", "TRolename");
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(int? id, HttpPostedFileBase TProfile, [Bind(Include = "TUsername,TPassword,TEmail,TMobile,TRoleid")] Usertable updatedUser)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Usertable existingUser = EventManagementEntities.Usertables.Find(id);

            if (existingUser == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                if (TProfile != null && TProfile.ContentLength > 0)
                {
                    byte[] profile;

                    using (var reader = new BinaryReader(TProfile.InputStream))
                    {
                        profile = reader.ReadBytes(TProfile.ContentLength);
                    }

                    existingUser.TProfile = profile;
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



