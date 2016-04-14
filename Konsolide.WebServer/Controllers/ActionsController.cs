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
        [ActionAttribute("Aksiyon Sayfası", "Sistemde kullanılan bütün aksiyonların(metodların) görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }
        [ActionAttribute("Aksiyonların Çağrılması", "Sistemde kullanılan bütün aksiyonların(metodların) getirilmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        public JsonResult GetActions(int pageSize, int skip)
        {
            try
            {
                var Actions = this.WebApplicationManager.Framework.Configuration.GetActiveActionList();
                var total = Actions.Count();
                var data = Actions.OrderBy(m => m.Id).Skip(skip).Take(pageSize).ToList();
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
