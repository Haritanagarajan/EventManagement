using EventManagement.Models;
using EventManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static EventManagement.Controllers.PaymentController;

namespace EventManagement.Controllers
{


    public class PaymentController : Controller
    {
        // GET: Payment
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public class OrderModel
        {
            public string orderId { get; set; }
            public string razorpayKey { get; set; }
            public int amount { get; set; }
            public string currency { get; set; }
            public string name { get; set; }

        }


        [HttpPost]
        public ActionResult CreateOrder(decimal totalCost)
        {

            // Generate random receipt number for order
            Random randomObj = new Random();
            string transactionId = randomObj.Next(10000000, 100000000).ToString();

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_bVkP46deaERqTV", "ADT4yd8Nm6yuGLFtyVtBOqcw");
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", totalCost * 100);
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0");
            Razorpay.Api.Order orderResponse = client.Order.Create(options);
            string orderId = orderResponse["id"].ToString();
            ViewData["totalCost"] = totalCost;
            int? userId = Session["UserId"] as int?;

            OrderModel orderModel = new OrderModel
            {
                orderId = orderResponse.Attributes["id"],
                razorpayKey = "rzp_test_bVkP46deaERqTV",
                amount = (int)totalCost * 100,
                currency = "INR",
                name = userId.Value.ToString(),
            };

            return View(orderModel);
        }


        [HttpPost]
        public ActionResult Complete()
        {

            string paymentId = Request.Params["rzp_paymentid"];

            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_bVkP46deaERqTV", "ADT4yd8Nm6yuGLFtyVtBOqcw");

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];


            if (paymentCaptured.Attributes["status"] == "captured")
            {
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }
    }
}



