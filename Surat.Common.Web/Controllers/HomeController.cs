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

        public ActionResult Spa()
        {

            try
            {
                ViewBag.Title = this.WebApplicationManager.Framework.Context.Product.CustomerProductName;
                ViewBag.SessionStart = this.WebApplicationManager.Framework.Context.CurrentUser.SessionStart;
            }
            catch (Exception)
            {
                RedirectToAction("Login", "Account");
            }
               
           
            return View();
        }

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