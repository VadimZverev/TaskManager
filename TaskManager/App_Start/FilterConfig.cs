using System.Web;
using System.Web.Mvc;
using TaskManager.Filters;

namespace TaskManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filters.AuthorizeAttribute());
        }
    }
}
