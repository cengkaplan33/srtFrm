using Surat.Business.Security;
using Surat.Common.Data;
using Surat.Common.Security;
using Surat.WebServer.ActionFilters;
using Surat.WebServer.Application;
using Surat.WebServer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Surat.WebServer.Controllers
{
    public class HomeController : SuratControllerBase
    {
        #region Constructor

        public HomeController()
        {
           
        }

        #endregion

        #region Private Members        
        
        #endregion

        #region Public Members

        #endregion

        #region Methods

        [ActionAttribute("Uygulama Başlangıç Anasayfası", "Uygulama başlangıç sayfasının görüntülenmesini sağlar ", Constants.Application.WebFrameworkSystemName, Surat.Common.Data.ActionType.Page)]
        [SuratAuthorizationFilter]
        public ActionResult Spa()
        {

            try
            {
                var sss = this.WebApplicationManager.Framework.Security.HasOperationRight(this.WebApplicationManager.Rights.WebAuditor.Id);
                //var sss = this.WebApplicationManager.Framework.Security.Rigths.AuditManagement1.Id;
                //this.WebApplicationManager..Framework
                var ssss = this.WebApplicationManager.Rights.WebAuditor.Id;

                ViewBag.Title = this.WebApplicationManager.Framework.Context.Product.CustomerProductName;
                ViewBag.SessionStart = this.WebApplicationManager.Framework.Context.CurrentUser.SessionStart;
            }
            catch (Exception)
            {
                RedirectToAction("Login", "Account");
            }
               
           
            return View();
        }

        [SuratAuthorizationFilter]
        [ActionAttribute("Portal Anasayfası", "Portal anasayfasının görüntülenmesini sağlar ", Constants.Application.WebFrameworkSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        #endregion

    }
}