using EventManagement.Models;
using EventManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventManagement.Controllers
{
    public class CartController : Controller
    {
        EventManagement2Entities1 EventManagementEntities = new EventManagement2Entities1();
        private readonly EventManagement2Entities1 db = new EventManagement2Entities1();

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
                babyshowertable = db.babyshowertables.ToList()

            };
            return View(carttables);
        }
    }
}