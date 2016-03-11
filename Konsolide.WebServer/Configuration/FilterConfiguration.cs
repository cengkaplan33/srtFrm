using Konsolide.WebServer.ActionFilters;
using System.Web;
using System.Web.Mvc;

namespace Surat.Web
{
    public static class FilterConfiguration
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogActionFilter());
            filters.Add(new KonsolideAuthorizationFilter());
        }
    }
}
