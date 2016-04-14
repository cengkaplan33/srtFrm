using Surat.Base.Model.Entities;
using Surat.Common.Security;
using Surat.Common.ViewModel;
using Surat.SerendipApplication.Business;
using Surat.WebServer.Application;
using Surat.WebServer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
namespace Surat.WebServer.Controllers
{
    public class AccountController : SuratControllerBase
    {
        #region Constructor

        public AccountController()
        {

        }

        #endregion

        #region Private Members

        #endregion

        #region Methods
        
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!string.IsNullOrEmpty(this.ExceptionMessage))
                ViewBag.Message = this.ExceptionMessage;

            //Set CurrentCulture
            this.WebApplicationManager.Context.CurrentCulture = this.WebApplicationManager.Framework.Globalization.Context.CurrentCulture;
            return View();
        }

        [AllowAnonymous]
        public JsonResult UserLogin(LoginView kullanici, string returnUrl)
        {
            try
            {
                this.WebApplicationManager.Login(kullanici.UserName, kullanici.Password);

                if (returnUrl == null)
                {
                    return Json(new { returnUrl = "/" });
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Json(new { returnUrl = returnUrl });
                }
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LogOut()
        {
            try
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                if (this.WebApplicationManager.Framework.Context.IsCurrentUserAssigned)
                    this.WebApplicationManager.Logout();
                //ToDo : else : Aktif session nasıl kapatılabilir.                       

                return Json(new { sonuc = "Çıkış işleminiz gerçekleştirildi" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { sonuc = this.PublishException(exception) });
            }
        }

        #endregion
    }
}
