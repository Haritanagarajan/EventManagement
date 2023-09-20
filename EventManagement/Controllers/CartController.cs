using EventManagement.Models;
using EventManagement.ViewModel;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;
using Razorpay.Api.Errors;


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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult PlaceOrder(int halfAmount, int PartialAmount, DateTime bookingdatetime)
        //{
        //    int? userId = Session["UserId"] as int?;
        //    int? eventId = Session["eventid"] as int?;
        //    string usermobile = Session["UserMobile"] as string;
        //    string useremail = Session["UserEmail"] as string;

        //    //Console.WriteLine(useremail);
        //    Console.WriteLine(usermobile);


        //    Razorpay.Api.RazorpayClient client = new RazorpayClient("rzp_test_D3KXHgdS7fmKuO", "GYl4qNswv7eZvy5RMzSoFen3");

        //    Dictionary<string, object> options = new Dictionary<string, object>
        //    {
        //    { "amount", halfAmount * 100},
        //    { "currency", "INR" },
        //    };


        //    bool num = true;
        //    string receiptId = "receipt_" + Guid.NewGuid().ToString().Substring(0, 32);
        //    options.Add("receipt", receiptId);

        //    Razorpay.Api.Order razorpayOrder = client.Order.Create(options);
        //    string razorpayOrderId = razorpayOrder["id"].ToString();
        //    var order = new Models.FinalPaymentReceived
        //    {
        //        paymentdatetime = DateTime.Now,
        //        userid = userId,
        //        razorpayid = razorpayOrderId,
        //        bookingdatetime = bookingdatetime,
        //        partialamount = PartialAmount,
        //        totalcost = halfAmount,
        //        eventid = eventId,
        //        ispaid = num,
        //        username = User.Identity.Name,
        //        usercontact = usermobile,
        //        usermail = useremail

        //    };
        //    EventManagementEntities.FinalPaymentReceiveds.Add(order);
        //    EventManagementEntities.SaveChanges();
        //    return View(order);
        //}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PlaceOrder(int halfAmount, int PartialAmount, DateTime bookingdatetime)
        {
            int? userId = Session["UserId"] as int?;
            int? eventId = Session["eventid"] as int?;


            var user = EventManagementEntities.Usertables.FirstOrDefault(x => x.TUserid == userId);
            string usermobile = user.TMobile.ToString();
            string useremail = user.TEmail;


            Console.WriteLine(useremail);
            Console.WriteLine(usermobile);

            Razorpay.Api.RazorpayClient client = new RazorpayClient("rzp_test_D3KXHgdS7fmKuO", "GYl4qNswv7eZvy5RMzSoFen3");

            Dictionary<string, object> options = new Dictionary<string, object>
    {
        { "amount", halfAmount * 100 },
        { "currency", "INR" },
    };

            bool num = true;
            string receiptId = "receipt_" + Guid.NewGuid().ToString().Substring(0, 32);
            options.Add("receipt", receiptId);

            try
            {
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
                    eventid = eventId,
                    ispaid = num,
                    username = User.Identity.Name,
                    usercontact = usermobile,
                    usermail = useremail

                };
                EventManagementEntities.FinalPaymentReceiveds.Add(order);
                EventManagementEntities.SaveChanges();
                return View(order);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Error204", "Error");
            }
        }



        public ActionResult Recipt(int? id)
        {
            FinalPaymentReceived final = EventManagementEntities.FinalPaymentReceiveds.Find(id);
            return View(final);
        }







        [HttpPost]
        public ActionResult Mailer(string razorpayOrderId)
        {

            var order = EventManagementEntities.FinalPaymentReceiveds.FirstOrDefault(o => o.razorpayid == razorpayOrderId);

            if (order == null)
            {
                return HttpNotFound();
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Harishmitha", "20bsca121harishmithak@skacas.ac.in"));
            message.To.Add(new MailboxAddress(order.Usertable.TUsername, order.Usertable.TEmail));
            message.Subject = "Order Confirmation";

            var bodyBuilder = new BodyBuilder
            {
                TextBody = "Thank you for placing your order with us. Your order details are as follows:\n\n" +
                            $"Order ID: {order.razorpayid}\n" +
                            $"Total Amount: ₹ {order.totalcost}\n" +
                            $"Partial Amount: {order.partialamount}\n" +
                            $"Event id: {order.eventid}\n" +
                            $"Booking Date: {order.bookingdatetime}"
            };


            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("20bsca121harishmithak@skacas.ac.in", "welcome123");
                client.Send(message);
                client.Disconnect(true);
            }


            return RedirectToAction("Index1");



        }

    }

}


