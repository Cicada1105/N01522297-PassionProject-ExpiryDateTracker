using System.Web;
using System.Web.Mvc;

namespace N01522297_PassionProject_ExpiryDateTracker
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
