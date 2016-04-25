﻿using Surat.Base.Model.Entities;
using Surat.Common.Security;
using Surat.Common.ViewModel;
using Surat.Common.Security;
using Surat.Common.Data;
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
        [ActionAttribute("Giriş Sayfası", "Sayfanın görüntülenmesini sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
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
        [ActionAttribute("Giriş Yap", "Sistemde kayıtlı olan kullanıcıların giriş yapmasını sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult UserLogin(LoginView kullanici, string returnUrl)
        {
            try
            {
                
                this.WebApplicationManager.Login(kullanici.UserName, kullanici.Password,kullanici.isActiveDirectoryUser);

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

        [AllowAnonymous]
        [ActionAttribute("Şifre Hatırla", "Sistemde kayıtlı olan kullanıcıların şifrelerini mail yoluyla hatırlatır.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult RememberPassword(EmailView email)
        {
            try
            {
                return Json(new { result = "Mail adresi: " + email.Email }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
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
