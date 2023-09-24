using EventManagement.Utility;
using System.Web;
using System.Web.Mvc;
using static EventManagement.MvcApplication;
using static EventManagement.Utility.CustomDeleteException;

namespace EventManagement
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //filters.Add(new MyFilter());

        }
    }
}
