using KonsolideRapor.Base.Model.Entities;
using KonsolideRapor.Common.ViewModel;
using KonsolideRapor.WebServer.Base;
using Surat.Common.Data;
using Surat.Common.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KonsolideRapor.WebServer.Controllers
{
    public class HazirDegerlerTablosuController : KonsolideControllerBase
    {
        #region Constructor

        public HazirDegerlerTablosuController()
        {

        }

        #endregion

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods
        [ActionAttribute("Banka Tablosu Sayfası", "Banka Tabloları sayfasının görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }
        
        [ActionAttribute("Banka Tablosu verilerinin Çağrılması", "Bankalara ait veri girişlerinin yapıldığı ekranı oluşturan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        public JsonResult GetHazirDegerler()
        {
            try
            {


                return Json(this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetHazirDegerlerList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionAttribute("Banka Tablosu Verilerinin Eklenmesi", "Bankalara ait veri girişlerinin eklenmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Add(HazirOdemeTablosuView hazirDegerTablosu)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SaveHazirDegerTablosu(hazirDegerTablosu);
                return Json(new { Result = "Kayıt işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Banka Tablosu Verilerinin Düzenlenmesi", "Bankalara ait veri girişlerinin düzenlenmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Update(HazirOdemeTablosuView hazirDegerTablosu)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SaveHazirDegerTablosu(hazirDegerTablosu);
                return Json(new { Result = "Güncelleme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }


        #endregion

    }
}
