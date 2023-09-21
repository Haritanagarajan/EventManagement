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
using System.Net;
using System.IO;
using Org.BouncyCastle.Utilities.Net;

namespace EventManagement.Controllers
{
    public class CartController : Controller
    {
        EventManagement2Entities2 EventManagementEntities = new EventManagement2Entities2();
        private readonly EventManagement2Entities2 db = new EventManagement2Entities2();

        // GET: Cart
      

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
        public ActionResult PlaceOrder(int halfAmount, int PartialAmount, DateTime bookingdatetime,int eventid)
        {
            int? userId = Session["UserId"] as int?;
            //int? eventId = Session["eventid"] as int?;
            string usermobile = Session["UserMobile"] as string;
            string useremail = Session["UserEmail"] as string;

            //Console.WriteLine(useremail);
            Console.WriteLine(usermobile);


            Razorpay.Api.RazorpayClient client = new RazorpayClient("rzp_test_D3KXHgdS7fmKuO", "GYl4qNswv7eZvy5RMzSoFen3");

            Dictionary<string, object> options = new Dictionary<string, object>
            {
            { "amount", PartialAmount * 100},
            { "currency", "INR" },
            };


            string receiptId = "receipt_" + Guid.NewGuid().ToString().Substring(0, 32);
            options.Add("receipt", receiptId);
            options.Add("payment_capture", 0);

            Razorpay.Api.Order razorpayOrder = client.Order.Create(options);
            string razorpayOrderId = razorpayOrder["id"].ToString();

            FinalPaymentReceived order = new Models.FinalPaymentReceived
            {
                paymentdatetime = DateTime.Now,
                userid = userId,
                razorpayid = razorpayOrderId,
                bookingdatetime = bookingdatetime,
                partialamount = PartialAmount,
                totalcost = halfAmount,
                eventid = eventid,
                username = User.Identity.Name,
                usercontact = usermobile,
                usermail = useremail

            };

            TempData["razorpayid"] = razorpayOrderId;
            TempData["totalcost"] = halfAmount;
            TempData["partialamount"] = PartialAmount;
            TempData["eventid"] = eventid;
            TempData["bookdatetime"] = bookingdatetime;



            return RedirectToAction("MiddlePayment",order);
        }

        public ActionResult MiddlePayment(Models.FinalPaymentReceived order)
        {
            return View(order);
        }

        [HttpPost]
        public ActionResult Complete(Models.FinalPaymentReceived order, string razorpayid)
        {
            int? totalcost = TempData["totalcost"] as int?;
            int? partialamount = TempData["partialamount"] as int?;
            int? eventid = TempData["eventid"] as int?;
            DateTime? bookingdatetime = TempData["bookdatetime"] as DateTime?;

            int? eventId = Session["eventid"] as int?;
            string usermobile = Session["UserMobile"] as string;
            string useremail = Session["UserEmail"] as string;


            string paymentId = Request.Params["rzp_paymentid"];

            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_D3KXHgdS7fmKuO", "GYl4qNswv7eZvy5RMzSoFen3");

            //Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            Payment payment = client.Payment.Fetch(paymentId);

            Dictionary<string, object> options = new Dictionary<string, object>
            {
            { "amount", partialamount * 100},
            { "currency", "INR" },
            };

            Payment paymentcaptured = payment.Capture(options);
            string amt = paymentcaptured["amount"].ToString();

            int? userId = Session["UserId"] as int?;

            if (paymentcaptured["status"].ToString() == "captured")
            {
                order.paymentdatetime = DateTime.Now;
                order.userid = userId;
                order.razorpayid = razorpayid;
                order.bookingdatetime = bookingdatetime;
                order.partialamount = partialamount;
                order.totalcost = totalcost;
                order.eventid = eventId;
                order.username = User.Identity.Name;
                order.usercontact = usermobile;
                order.usermail = useremail;
                order.ispaid = true;

                RemoveCartDetails(order.userid);
                EventManagementEntities.FinalPaymentReceiveds.Add(order);
                EventManagementEntities.SaveChanges();
            }
            else
            {
                return RedirectToAction("Error204","Error");
            }


            return RedirectToAction("Recipt", new { id =  order.id});

        }



        public ActionResult Recipt(int? id)
        {
            FinalPaymentReceived final = EventManagementEntities.FinalPaymentReceiveds.Find(id);
            return View(final);
        }


        public ActionResult RemoveCartDetails(int? id)
        {
            var baby = EventManagementEntities.babyshowertables.Where(x=>x.babyshoweruserid == id).FirstOrDefault();
            var bday = EventManagementEntities.birthdaytables.Where(x => x.bdayuserid == id).FirstOrDefault();
            var reunion = EventManagementEntities.Reunions.Where(x => x.reunionuserid == id).FirstOrDefault();
            var bachelor = EventManagementEntities.BachelorParties.Where(x => x.bacheloruserid == id).FirstOrDefault();
            var wedding = EventManagementEntities.Weddings.Where(x => x.weddinguserid == id).FirstOrDefault();
            var anni = EventManagementEntities.Anniversaries.Where(x => x.anniuserid == id).FirstOrDefault();
            var cock = EventManagementEntities.CocktailParties.Where(x => x.cockuserid == id).FirstOrDefault();


            if (baby != null || bday != null || reunion != null || bachelor != null || wedding != null || anni != null || cock != null)
            {
                EventManagementEntities.babyshowertables.Remove(baby);
                EventManagementEntities.birthdaytables.Remove(bday);
                EventManagementEntities.Reunions.Remove(reunion);
                EventManagementEntities.BachelorParties.Remove(bachelor);
                EventManagementEntities.Weddings.Remove(wedding);
                EventManagementEntities.Anniversaries.Remove(anni);
                EventManagementEntities.CocktailParties.Remove(cock);
                EventManagementEntities.SaveChanges();

            }


            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Mailer(string razorpayOrderId)
        {

            var order = EventManagementEntities.FinalPaymentReceiveds.FirstOrDefault(o => o.razorpayid == razorpayOrderId);

            if (order == null)
            {
                return HttpNotFound();
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("VIGNESH", "20bsca150vigneshr@skacas.ac.in"));
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
                client.Authenticate("20bsca150vigneshr@skacas.ac.in", "welcome123");
                client.Send(message);
                client.Disconnect(true);
            }


            return RedirectToAction("Index1");

        }




        //public ActionResult Milerview()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Mailer(EmailModel model)
        //{
        //    string receiver = Session["UserEmail"] as string;

        //    using (MailMessage mm = new MailMessage(model.Email, receiver))
        //    {
        //        mm.Subject = model.Subject;
        //        mm.Body = model.Body;
        //        if (model.Attachment.ContentLength > 0)
        //        {
        //            string fileName = Path.GetFileName(model.Attachment.FileName);
        //            mm.Attachments.Add(new Attachment(model.Attachment.InputStream, fileName));
        //        }
        //        mm.IsBodyHtml = false;
        //        using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
        //        {
        //            smtp.Host = "smtp.gmail.com"; //IP Address
        //            smtp.EnableSsl = true; //false
        //            smtp.UseDefaultCredentials = false;
        //            NetworkCredential NetworkCred = new NetworkCredential(model.Email, model.Password);
        //            //NetworkCredential NetworkCred = new NetworkCredential("Mail ID", "Password");

        //            smtp.Credentials = NetworkCred;
        //            smtp.Port = 587; //25
        //            //Turn Less Secure apps to "ON" in your Sender Google Account
        //            smtp.Send(mm);
        //            ViewBag.Message = "Email sent.";
        //        }
        //    }

        //    return View();
        //}

    }

}


