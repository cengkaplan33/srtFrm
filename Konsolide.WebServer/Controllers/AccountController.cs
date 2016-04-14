﻿using Surat.Base.Model.Entities;
using Surat.Common.ViewModel;
using Surat.Common.Security;
using Surat.Common.Data;
using KonsolideRapor.WebServer.Application;
using KonsolideRapor.WebServer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
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

        [AllowAnonymous]
        [ActionAttribute("Giriş Sayfası", "Sayfanın görüntülenmesini sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
        public ActionResult Login(string returnUrl)
        {            
            
            ViewBag.ReturnUrl = returnUrl;
            if (!string.IsNullOrEmpty(this.ExceptionMessage))
                ViewBag.Message = this.ExceptionMessage;

            //Set CurrentCulture
            this.WebApplicationManager.Context.CurrentCulture = this.WebApplicationManager.KonsolideRapor.Framework.Globalization.Context.CurrentCulture;
            return View();
        }

        [AllowAnonymous]
        [ActionAttribute("Giriş Yap", "Sistemde kayıtlı olan kullanıcıların giriş yapmasını sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
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

        [ActionAttribute("Çıkış Yap", "Giriş yapmış kullanıcıların çıkış yapmasını sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
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
