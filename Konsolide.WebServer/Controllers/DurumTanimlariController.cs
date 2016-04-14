using KonsolideRapor.Base.Model.Entities;
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
        [ActionAttribute("Durum Tanımları Sayfası", "Durum Tanımları sayfasının görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Durum Tanımları Düzenleme Sayfası", "Durum Tanımlarının düzenlendiği sayfanın görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Edit()
        {
            return View();
        }

        [ActionAttribute("Durum Tanımlarının Çağrılması", "Sistemde kullanılan bütün durum tanımlarının  getirilmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
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

        [ActionAttribute("Ödeme Ekranı-Durum Tanımlarının Çağrılması", "Sistemde kullanılan bütün ödeme ekranı durum tanımlarının  getirilmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
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

        [ActionAttribute("Tahsilat Ekranı-Durum Tanımlarının Çağrılması", "Sistemde kullanılan bütün tahsilat ekranı durum tanımlarının  getirilmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
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

        [ActionAttribute("Durum Tanımı Eklenmesi", "Sisteme yeni bir durum tanımı yapılmasını sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
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

        [ActionAttribute("Durum Tanımı Düzenlenmesi", "Sistemde seçilen bir durum tanımının düzenlenmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
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

        [ActionAttribute("Durum Tanımı Silinmesi", "Sistemde seçilen bir durum tanımının silinmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
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
