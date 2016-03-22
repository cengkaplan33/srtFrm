using Surat.Business.Log;
using Surat.Common.Data;
using Surat.Web.Common;
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
    public class SuratAuthorizationFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string action = MVCUtility.GetControllerName() + "/" + MVCUtility.GetControllerActionName();

            if (!MVCUtility.IsUnAuthorizedAction(action))
                base.OnAuthorization(filterContext);
            else return;
            
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)           
            {
                SuratControllerBase controller = filterContext.Controller as SuratControllerBase;                
                try
                {
                    //Session timeout kontrolü - CurrentUser bulunamaz.                       
                    if (!controller.WebApplicationManager.Framework.Security.ApplicationContext.IsCurrentUserAssigned)
                    {

                        filterContext.Result = new RedirectResult(Constants.Web.RedirectLogoutAction); //ToDo : Client tarafından sunucuya istek yapılmıyor. Yapılırsa, burası tekrar ele alınmalıdır.
                        return;
                    }
                    else if ( !controller.WebApplicationManager.Framework.Security.HasRight(action))
                    {
                        filterContext.Result =  new System.Web.Mvc.JsonResult(){ Data ="weqw ewq qwe " };
                        
                        //filterContext.Controller.TempData.Add("RedirectReason", "Unauthorized");

                        //filterContext.Result = new HttpUnauthorizedResult("yetkin yok") ;
                        return;

                       //return new ActionResult ( () => 
                       //     JsonResult()
                       // MVCUtility.GetControllerName(), controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId,WebConstants.Message.WebSessionTimeout), TraceLevel.Basic);
                       // );
                        
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
