using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace EventManagement.Utility
{
    public class CustomException: Exception
    {

        public CustomException() : base("Should Not Contain Digits!")
        { 
        }

    }

}