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
        public SuratAuthorizationFilter()
        {
            this.Permission = null;
            this.AccessType = null;
        }

        public SuratAuthorizationFilter(object permission)
            : this()
        {
            this.Permission = permission == null ? null : permission.ToString();
            this.AccessType = null;
        }

        public SuratAuthorizationFilter(Surat.Business.Security.SuratRight Right)
            : this()
        {
            this.Permission = Right == null ? null : Right.Key.ToString();
            this.AccessType = AccessibleItemDBObjectType.Right;
        }

        public SuratAuthorizationFilter(string AccessName, AccessibleItemDBObjectType AccessType)
            : this()
        {
            this.Permission = AccessName == null ? null : AccessName.ToString();
            this.AccessType = AccessType;
        }

        public string Permission { get; private set; }
        public AccessibleItemDBObjectType? AccessType { get; private set; }


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

                        //filterContext.Result = new RedirectResult(Constants.Web.RedirectLogoutAction); //ToDo : Client tarafından sunucuya istek yapılmıyor. Yapılırsa, burası tekrar ele alınmalıdır.
                        filterContext.HttpContext.Response.StatusCode = 500;
                        filterContext.Result = new System.Web.Mvc.JsonResult() { Data = new { Status = "RedirectToLogin", Message = controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId, Constants.Message.RedirectToLogin) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        return;
                    }
                    else
                    {
                        bool isAuthorized = false;

                        if (Permission == null)
                            isAuthorized = controller.WebApplicationManager.Framework.Security.HasRight(action);
                        else if (AccessType == null)
                            isAuthorized = controller.WebApplicationManager.Framework.Security.HasRight(Permission);
                        else
                            isAuthorized = controller.WebApplicationManager.Framework.Security.HasRight(Permission, AccessType.Value);

                        if (!isAuthorized)
                        {
                            filterContext.HttpContext.Response.StatusCode = 500;
                            filterContext.Result = new System.Web.Mvc.JsonResult() { Data = new { Status = "AccessDenied", Message = String.Format(controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId, Constants.Message.UserAccessDenied), action, controller.WebApplicationManager.Context.CurrentUser.Name) }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                            //filterContext.Controller.TempData.Add("RedirectReason", "Unauthorized");

                            //filterContext.Result = new HttpUnauthorizedResult("yetkin yok") ;
                            return;

                            //return new ActionResult ( () => 
                            //     JsonResult()
                            // MVCUtility.GetControllerName(), controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId,WebConstants.Message.WebSessionTimeout), TraceLevel.Basic);
                            // );

                        }
                    }
                }
                catch (Exception exception)
                {
                    //filterContext.Result = new RedirectResult(Constants.Web.RedirectLoginAction); //ToDo : Hata olduğunda, genel bir mesaj vermelidir.
                    //controller.WebApplicationManager.TraceAppendLine(MVCUtility.GetControllerName(), controller.WebApplicationManager.GetGlobalizationKeyValue(controller.WebApplicationManager.Context.SystemId, WebConstants.Message.WebSessionTimeout), TraceLevel.Basic);
                    //controller.WebApplicationManager.PublishException(exception);

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