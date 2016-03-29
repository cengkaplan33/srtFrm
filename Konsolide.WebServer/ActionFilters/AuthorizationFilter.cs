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
                    if (!controller.WebApplicationManager.Framework.Security.ApplicationContext.IsCurrentUserAssigned)
                    {
                        filterContext.HttpContext.Response.StatusCode = 500;
                        filterContext.Result = new System.Web.Mvc.JsonResult() { Data = new { Status = "RedirectToLogin", Message = controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId, Constants.Message.RedirectToLogin) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        return;
                    }
                    else if (!controller.WebApplicationManager.Framework.Security.HasRight(action))
                    {
                        filterContext.HttpContext.Response.StatusCode = 500;
                        filterContext.Result = new System.Web.Mvc.JsonResult() { Data = new { Status = "AccessDenied", Message = String.Format(controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId, Constants.Message.UserAccessDenied), action, controller.WebApplicationManager.KonsolideRapor.Context.CurrentUser.Name) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                        //filterContext.Controller.TempData.Add("RedirectReason", "Unauthorized");

                        //filterContext.Result = new HttpUnauthorizedResult("yetkin yok") ;
                        return;

                        //return new ActionResult ( () => 
                        //     JsonResult()
                        // MVCUtility.GetControllerName(), controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId,WebConstants.Message.WebSessionTimeout), TraceLevel.Basic);
                        // );

                    }

                    ////Session timeout kontrolü - CurrentUser bulunamaz.                                  
                    //if (!controller.WebApplicationManager.Framework.Security.ApplicationContext.IsCurrentUserAssigned ||
                    //    !controller.WebApplicationManager.Framework.Security.HasActionRight(action))
                    //{
                    //    filterContext.Result = new RedirectResult(Constants.Web.RedirectLogoutAction); //ToDo : Client tarafından sunucuya istek yapılmıyor. Yapılırsa, burası tekrar ele alınmalıdır.
                    //    return;
                    //}
                }
                catch (Exception exception)
                {
                    //  filterContext.Result = new RedirectResult(Constants.Web.RedirectLoginAction); //ToDo : Hata olduğunda, genel bir mesaj vermelidir.

                    filterContext.HttpContext.Response.StatusCode = 500;
                    filterContext.Result = new System.Web.Mvc.JsonResult() { Data = new { Status = "KonsolideAuthorizationFilterError", Message = exception.Message }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                    controller.WebApplicationManager.TraceAppendLine(MVCUtility.GetControllerName(), exception.Message, TraceLevel.Basic);
                    controller.WebApplicationManager.PublishException(exception);
                    return;
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