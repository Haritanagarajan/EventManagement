using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using EventManagement.Models;
using EventManagement.Models.DAL;
using System.IO;
using static EventManagement.MvcApplication;


using EventManagement.Utility;

namespace EventManagement.Controllers
{

    public class FeedbackController : Controller
    {

        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();

        string Baseurl = "https://localhost:44341/"; //use webapi port
                                                     // GET: Feedback



        [Authorize(Roles = "User")]
        public async Task<ActionResult> Index()
        {
            List<feedbacktable> feed = new List<feedbacktable>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/feedbacktables");
                if (Res.IsSuccessStatusCode)
                {
                    var FeedResponse = Res.Content.ReadAsStringAsync().Result;
                    feed = JsonConvert.DeserializeObject<List<feedbacktable>>(FeedResponse);
                }
                return View(feed);
            }

        }

        private IFeedBackRepocs _employeeRepository;

        public FeedbackController()
        {
            this._employeeRepository = new FeedBackRepo(new EventManagement2Entities2());
        }





        [Authorize(Roles = "User")]

        [MyFilter]
        public ActionResult Details(int id)
        {
            return View(_employeeRepository.GetEmployeeById(id));
        }

        public ActionResult Create()
        {
            return View(new feedbacktable());
        }





        [HttpPost]
        [Authorize(Roles = "User")]

        public ActionResult Create(feedbacktable employee)
        {

            int? userId = Session["UserId"] as int?;

            employee.UserId = userId;

            _employeeRepository.InsertEmployee(employee);

            _employeeRepository.SaveChanges();
            return RedirectToAction("ThankYou","Cart");
        }
        public ActionResult Update(int Id)
        {

            return View(_employeeRepository.GetEmployeeById(Id));
        }
        [HttpPost]
        public ActionResult Update(feedbacktable employee)
        {
            int? userId = Session["UserId"] as int?;

            employee.UserId = userId;

            _employeeRepository.UpdateEmployee(employee);
            _employeeRepository.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int Id)
        {
            _employeeRepository.DeleteEmployee(Id);
            _employeeRepository.SaveChanges();
            return RedirectToAction("Index");
        }



        //public ActionResult Sample(int? num)
        //{
        //    try
        //    {
        //        if (num < 0)
        //        {
        //            return Content("num value is:" + num);
        //        }
        //        else
        //        {
        //            throw new CustomException();
        //        }
        //    }
        //    catch (CustomException ex)
        //    {
        //        return Content("Custom Exception: " + ex.Message);
        //    }
        //}


    }
}





