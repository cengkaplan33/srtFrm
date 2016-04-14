using Surat.Base.Model.Entities;
using Surat.Common.ViewModel;
using KonsolideRapor.WebServer.Application;
using KonsolideRapor.WebServer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Surat.Common.Security;
namespace KonsolideRapor.WebServer.Controllers
{
    public class AccountController : KonsolideControllerBase
    {
        #region Constructor

        public AccountController()
        {
            
        }

        #endregion

        #region Private Members        

        #endregion

        #region Methods       

        [ActionAttribute("Giriş Sayfası", "Kullanıcıların portala giriş yaptığı sayfayı görüntüler", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName,Surat.Common.Data.ActionType.Page)]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {            
            
            ViewBag.ReturnUrl = returnUrl;
            if (!string.IsNullOrEmpty(this.ExceptionMessage))
                ViewBag.Message = this.ExceptionMessage;

            //Set CurrentCulture
            this.WebApplicationManager.Context.CurrentCulture = this.WebApplicationManager.KonsolideRapor.Framework.Globalization.Context.CurrentCulture;
            return View();
        }

        [ActionAttribute("Kullanıcı Girişi", "Kullanıcının sistemde kayıtlı olup olmadığını kontrol eder", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [AllowAnonymous]
        public JsonResult UserLogin(LoginView kullanici, string returnUrl)
        {            
            try
            {
                this.WebApplicationManager.Login(kullanici.UserName, kullanici.Password);

                if (returnUrl == null)
                {
                    return Json(new { returnUrl = "/" },JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Json(new { returnUrl = returnUrl },JsonRequestBehavior.AllowGet);                    
                }
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception)},JsonRequestBehavior.AllowGet);
            }            
        }
        [ActionAttribute("Kullanıcı Çıkışı", "Kullanıcının sistemde ki oturumunun sonlandırılmasını sağlar", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        public ActionResult LogOut()
        {
            try
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                if (this.WebApplicationManager.KonsolideRapor.Framework.Context.IsCurrentUserAssigned)
                    this.WebApplicationManager.Logout();
                //ToDo : else : Aktif session nasıl kapatılabilir.                       
                
                return Json(new { sonuc="Çıkış işleminiz gerçekleştirildi"},JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { sonuc = this.PublishException(exception)},JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
