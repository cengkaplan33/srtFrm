using Surat.Business.Log;
using Surat.Common.Data;
using KonsolideRapor.Web.Common;
using KonsolideRapor.WebServer.Base;
using KonsolideRapor.WebServer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace KonsolideRapor.WebServer.ActionFilters
{
    public class KonsolideAuthorizationFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string action = MVCUtility.GetControllerName() + "/" + MVCUtility.GetControllerActionName();

            if (!MVCUtility.IsUnAuthorizedAction(action))
                base.OnAuthorization(filterContext);
            else return;
            
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)           
            {
               KonsolideControllerBase controller = filterContext.Controller as KonsolideControllerBase;                
                try
                {
                    //Session timeout kontrolü - CurrentUser bulunamaz.                                  
                    if (!controller.WebApplicationManager.KonsolideRapor.Framework.Security.ApplicationContext.IsCurrentUserAssigned ||
                        !controller.WebApplicationManager.KonsolideRapor.Framework.Security.HasActionRight(action))
                    {
                      filterContext.Result = new RedirectResult(Constants.Web.RedirectLogoutAction); //ToDo : Client tarafından sunucuya istek yapılmıyor. Yapılırsa, burası tekrar ele alınmalıdır.
                        return;
                    }
                }
                catch (Exception exception)
                {
                    filterContext.Result = new RedirectResult(Constants.Web.RedirectLoginAction); //ToDo : Hata olduğunda, genel bir mesaj vermelidir.
                    controller.WebApplicationManager.TraceAppendLine(MVCUtility.GetControllerName(), controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId,WebConstants.Message.WebSessionTimeout), TraceLevel.Basic);
                    controller.WebApplicationManager.PublishException(exception);                        
                }                                
            }
            else
            {
                filterContext.Result = new RedirectResult(Constants.Web.RedirectLoginAction);
                return;
            }
        }
    }
}
