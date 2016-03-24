using KonsolideRapor.Base.Model.Entities;
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
    public class DurumTanimlariController : KonsolideControllerBase
    {
         #region Constructor

        public DurumTanimlariController()
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

        public ActionResult Edit()
        {
            return View();
        }
    
        public JsonResult GetDurumTanimlari(int pageSize, int skip)
        {
            try
            {
                var paymentCollecting = this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetActiveDurumTanimlari();
                var total = paymentCollecting.Count();
                var data = paymentCollecting.OrderBy(m => m.Id).Skip(skip).Take(pageSize).ToList();
                return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOdemeEkraniDurumTanimlari()
        {
            try
            {

                return Json(this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetActiveOdemeDurumTanimlari(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTahsilatEkraniDurumTanimlari()
        {
            try
            {

                return Json(this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetActiveTahsilatDurumTanimlari(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Add(OdemeTalepDurumu konsolideState)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SaveDurumTanimi(konsolideState);
                return Json(new { Result = "Kayıt işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Update(OdemeTalepDurumu konsolideState)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SaveDurumTanimi(konsolideState);
                return Json(new { Result = "Güncelleme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Delete(OdemeTalepDurumu konsolideState)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.DestroyDurumTanimi(konsolideState);
                return Json(new { Result = "Silme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
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
