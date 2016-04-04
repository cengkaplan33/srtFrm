using KonsolideRapor.Base.Model.Entities;
using KonsolideRapor.Common.ViewModel;
using KonsolideRapor.WebServer.Base;
using Surat.Common.Data;
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
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Tanim()
        {
            return View();
        }
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
