using KonsolideRapor.WebServer.Base;
using Surat.Base.Model.Entities;
using Surat.Common.Data;
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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit()
        {
            return View();
        }
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
