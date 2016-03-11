using Konsolide.WebServer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Konsolide.WebServer.Controllers
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

           
            #endregion

        }
  
}
