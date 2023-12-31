﻿using EventManagement.Models;
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
        [Authorize(Roles = "User")]

        public ActionResult PlaceOrder(int halfAmount, int PartialAmount, DateTime bookingdatetime)
        {
            int? userId = Session["UserId"] as int?;
            int? eventId = Session["eventid"] as int?;
            long? usermobile = Session["UserContact"] as long?;
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
                eventid = eventId,
                username = User.Identity.Name,
                usercontact = usermobile,
                usermail = useremail

            };

            TempData["razorpayid"] = razorpayOrderId;
            TempData["totalcost"] = halfAmount;
            TempData["partialamount"] = PartialAmount;
            TempData["eventid"] = eventId;
            TempData["bookdatetime"] = bookingdatetime;



            return RedirectToAction("MiddlePayment", order);
        }


        [Authorize(Roles = "User")]

        public ActionResult MiddlePayment(Models.FinalPaymentReceived order)
        {
            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = "User")]

        public ActionResult Complete(Models.FinalPaymentReceived order, string razorpayid)
        {
            int? totalcost = TempData["totalcost"] as int?;
            int? partialamount = TempData["partialamount"] as int?;
            int? eventid = TempData["eventid"] as int?;
            DateTime? bookingdatetime = TempData["bookdatetime"] as DateTime?;

            int? eventId = Session["eventid"] as int?;
            long? usermobile = Session["UserContact"] as long?;
            string useremail = Session["UserEmail"] as string;


            string paymentId = Request.Params["rzp_paymentid"];

            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_D3KXHgdS7fmKuO", "GYl4qNswv7eZvy5RMzSoFen3");

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

                EventManagementEntities.FinalPaymentReceiveds.Add(order);
                EventManagementEntities.SaveChanges();
            }
            else
            {
                return RedirectToAction("Error204", "Error");
            }


            return RedirectToAction("Recipt", new { id = order.id });

        }


        [Authorize(Roles = "User")]
        public ActionResult Recipt(int? id)
        {
            FinalPaymentReceived final = EventManagementEntities.FinalPaymentReceiveds.Find(id);
            return View(final);
        }

        [HttpPost]
        [Authorize(Roles = "User")]
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


            return RedirectToAction("Create", "Feedback");

        }

        [Authorize(Roles = "User")]

        public ActionResult ThankYou()
        {
            return View();
        }


    }

}


