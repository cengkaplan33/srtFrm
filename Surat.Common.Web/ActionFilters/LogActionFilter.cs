using Surat.Business.Log;
using Surat.Common.Data;
using Surat.WebServer.Base;
using Surat.WebServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Surat.WebServer.ActionFilters
{
    public class LogActionFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            StringBuilder logText = new StringBuilder();
            if (filterContext.Controller is SuratControllerBase)
            {
                try
                {
                    SuratControllerBase controller = filterContext.Controller as SuratControllerBase;

                    logText.Append("UserName=" + filterContext.HttpContext.User.Identity.Name);
                    logText.Append(" Action=" + MVCUtility.GetControllerActionName());

                    if (filterContext.HttpContext.Request.QueryString.Count > 0)
                        logText.Append(" QueryParameters " + MVCUtility.GetQueryStringParameters(filterContext.HttpContext.Request.QueryString));

                    controller.WebApplicationManager.TraceAppendLine(MVCUtility.GetControllerName(), logText.ToString(), TraceLevel.Detail);
                }
                catch (Exception exception)
                {                    
                    throw exception; //ToDo : Ele alınmalıdır.
                }
                
            }
            
            this.OnActionExecuted(filterContext);
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller is SuratControllerBase)
            {
                try
                {
                    SuratControllerBase controller = filterContext.Controller as SuratControllerBase;
                    controller.WebApplicationManager.TraceAppendLine(MVCUtility.GetControllerName(), MVCUtility.GetControllerActionName(), TraceLevel.Detail);
                }
                catch (Exception exception)
                {
                    throw exception; //ToDo : Ele alınmalıdır.
                }                
            }

            this.OnActionExecuting(filterContext);
        }
    }
}
