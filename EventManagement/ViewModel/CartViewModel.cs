using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventManagement.ViewModel
{
    public class CartViewModel
    {

        public IEnumerable<birthdaytable> birthdaytable { get; set; }
        public IEnumerable<Anniversary> Anniversary { get; set; }
        public IEnumerable<babyshowertable> babyshowertable { get; set; }
        public IEnumerable<Reunion> Reunion { get; set; }
        public IEnumerable<CocktailParty> CocktailParty { get; set; }
        public IEnumerable<BachelorParty> BachelorParty { get; set; }
        public IEnumerable<Wedding> Wedding { get; set; }

    }
}