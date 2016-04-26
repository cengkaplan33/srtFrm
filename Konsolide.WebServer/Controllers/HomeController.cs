using KonsolideRapor.WebServer.Base;
using Surat.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KonsolideRapor.WebServer.Controllers
{

    public class HomeController : KonsolideControllerBase
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
        [ActionAttribute("Portal Anasayfası", "Portal anasayfasının görüntülenmesini sağlar ", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Uygulama Başlangıç Anasayfası", "Uygulama başlangıç sayfasının görüntülenmesini sağlar ", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, Surat.Common.Data.ActionType.Page)]
        [AllowAnonymous]
        public ActionResult Spa()
        {

            try
            {

                ViewBag.Title = this.WebApplicationManager.KonsolideRapor.Framework.Context.Product.CustomerProductName;
                ViewBag.SessionStart = this.WebApplicationManager.KonsolideRapor.Framework.Context.CurrentUser.SessionStart;

            }
            catch (Exception)
            {
                RedirectToAction("Login", "Account");
            }


            return View();
        }

        [ActionAttribute("Uygulama Başlangıç Anasayfası", "Uygulama başlangıç sayfasının görüntülenmesini sağlar ", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, Surat.Common.Data.ActionType.Page)]
        [AllowAnonymous]
        public ActionResult Anasayfa()
        {

            try
            {

                ViewBag.Title = this.WebApplicationManager.KonsolideRapor.Framework.Context.Product.CustomerProductName;
                ViewBag.SessionStart = this.WebApplicationManager.KonsolideRapor.Framework.Context.CurrentUser.SessionStart;

            }
            catch (Exception)
            {
                RedirectToAction("Login", "Account");
            }


            return View();
        }
        #endregion

    }

}
