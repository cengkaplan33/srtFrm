using Surat.Base.Model.Entities;
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
    public class SerendipSatisSiparisController : SuratControllerBase
    {
        #region Methods

        [ActionAttribute("Satış Siparişleri Liste Sayfası", "Satış siparişlerinin listelendiği sayfa.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Satış Siparişleri Listesi", "Satış siparişlerini liste olarak getirir.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult SatisSiparisleri(int pageSize, int skip)
        {
            try
            {
                this.Serendip.Login();
                var list = this.Serendip.SatisSiparisleri();

                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
