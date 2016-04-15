using Surat.Base.Model.Entities;
using Surat.Common.ViewModel;
using Surat.Common.Data;
using Surat.Common.Security;
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
    public class ActionsController : KonsolideControllerBase
    {
        #region Constructor

        public ActionsController()
        {

        }

        #endregion

        #region Private Members


        #endregion

        #region Public Members

        #endregion

        #region Methods
        [ActionAttribute("Aksiyon Sayfası", "Sayfanın görüntülenmesini sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }
        [ActionAttribute("Aksiyonları Getir", "Sistemde kayıtlı olan tüm aktif aksiyonları getirir.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult GetActions(int pageSize, int skip)
        {
            try
            {
                var Actions = this.WebApplicationManager.Framework.Configuration.GetActiveActionList();
                var total = Actions.Count();
                var data = Actions.OrderBy(m => m.Name).Skip(skip).Take(pageSize).ToList();
                return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);

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
