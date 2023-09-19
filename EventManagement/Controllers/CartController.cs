using EventManagement.Models;
using EventManagement.ViewModel;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    public class CartController : Controller
    {
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();
        private readonly EventManagement2Entities2 db = new EventManagement2Entities2();

        // GET: Cart
        public ActionResult Index()
        {
            List<AddtoCart> cart = EventManagementEntities.AddtoCarts.ToList();

            return View(cart);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public ActionResult Index1()
        {

            var carttables = new CartViewModel
            {

                birthdaytable = db.birthdaytables.ToList(),
                Wedding = db.Weddings.ToList(),
                Anniversary = db.Anniversaries.ToList(),
                Reunion = db.Reunions.ToList(),
                CocktailParty = db.CocktailParties.ToList(),
                BachelorParty = db.BachelorParties.ToList(),
                babyshowertable = db.babyshowertables.ToList(),
                Final = db.FinalPaymentReceiveds.ToList(),
            };

            return View(carttables);
        }


     

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(int halfAmount,int PartialAmount,DateTime bookingdatetime)
        {
            int? userId = Session["UserId"] as int?;
            int? eventId = Session["eventid"] as int?;
            string usermobile = Session["UserMobile"] as string;
            string useremail = Session["UserEmail"] as string;

            Console.WriteLine(useremail);
            Console.WriteLine(usermobile);


            Razorpay.Api.RazorpayClient client = new RazorpayClient("rzp_test_D3KXHgdS7fmKuO", "GYl4qNswv7eZvy5RMzSoFen3");

            Dictionary<string, object> options = new Dictionary<string, object>
            {
            { "amount", halfAmount * 100},
            { "currency", "INR" },
            };

            bool num = true;
            string receiptId = "receipt_" + Guid.NewGuid().ToString().Substring(0, 32);
            options.Add("receipt", receiptId);

            Razorpay.Api.Order razorpayOrder = client.Order.Create(options);
            string razorpayOrderId = razorpayOrder["id"].ToString();
            var order = new Models.FinalPaymentReceived
            {
                paymentdatetime = DateTime.Now,
                userid = userId,
                razorpayid = razorpayOrderId,
                bookingdatetime = bookingdatetime,
                partialamount = PartialAmount,
                totalcost = halfAmount,
                eventid= eventId,
                ispaid = num,
                username = User.Identity.Name,
                usercontact = usermobile,
                usermail = useremail

            };
            EventManagementEntities.FinalPaymentReceiveds.Add(order);
            EventManagementEntities.SaveChanges();
            return View(order);
        }


        public ActionResult Recipt()
        {
            FinalPaymentReceived model = new FinalPaymentReceived();


            return View(model);
        }

    }
}