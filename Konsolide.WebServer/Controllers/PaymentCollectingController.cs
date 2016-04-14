using KonsolideRapor.WebServer.Base;
using Surat.Base.Model.Entities;
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
    public class PaymentCollectingController : KonsolideControllerBase
    {
        #region Constructor

        public PaymentCollectingController()
        {

        }

        #endregion

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods

        [ActionAttribute("Ödeme Türleri Sayfası", "Ödeme tanımı sayfasının görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Ödeme Türleri Sayfasının Düzenlenmesi", "Ödeme türü sayfasının düzenlenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Edit()
        {
            return View();
        }

        [ActionAttribute("Aktif Ödeme Türleri Getirilmesi", "Sistemde ki aktif ödeme türlerinin getirilmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        public JsonResult GetOdemeTurleri()
        {
            try
            {

                return Json(this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetOdemeTurleri(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionAttribute("Tahsilat Türleri Tanımlarının Getirilmesi", "Sistemde ki tahsilat türleri tanımlarının getirilmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        public JsonResult GetTahsilatTurleri()
        {
            try
            {

                return Json(this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetTahsilatTurleri(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionAttribute("Ödeme Türleri Tanımlarının Getirilmesi", "Sistemde ki tahsilat türleri tanımlarının getirilmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        public JsonResult GetPaymentCollectings(int pageSize, int skip)
        {
            try
            {
                var paymentCollecting = this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetActivePaymentCollectingList();
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

        [ActionAttribute("Ödeme Türlerinin Eklenmesi", "Sistemde yeni bir ödeme türünün eklenmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Add(PaymentCollecting paymentCollecting)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SavePaymentCollecting(paymentCollecting);
                return Json(new { Result = "Kayıt işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Ödeme Türlerinin Düzenlenmesi", "Sistemde seçilen bir ödeme türünün seçilmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Update(PaymentCollecting paymentCollecting)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SavePaymentCollecting(paymentCollecting);
                return Json(new { Result = "Güncelleme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Ödeme Türlerinin Silinmesi", "Sistemde seçilen bir ödeme türünün silinmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Delete(PaymentCollecting paymentCollecting)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.DestroyPaymentCollecting(paymentCollecting);
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
