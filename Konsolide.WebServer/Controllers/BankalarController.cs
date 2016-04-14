using KonsolideRapor.Base.Model.Entities;
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
    public class BankalarController : KonsolideControllerBase
    {
        #region Constructor

        public BankalarController()
        {
            
        }

        #endregion

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods
        [ActionAttribute("Banka Sayfası", "Banka sayfasının görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }
        [ActionAttribute("Banka Düzenleme Sayfası", "Banka bilgilerinin düzenlendiği sayfasının görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Edit()
        {
            return View();
        }

        [ActionAttribute("Bankaların Çağrılması", "Sistemde kullanılan bütün bankaların  getirilmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        public JsonResult GetBanks(int pageSize, int skip)
        {
            try
            {
                var banks = this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetActiveBankList();
                var total = banks.Count();
                var data = banks.OrderBy(m => m.Id).Skip(skip).Take(pageSize).ToList();
                return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);
              
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }  
        }

        [ActionAttribute("Banka Eklenmesi", "Sisteme yeni bir banka tanımı yapılmasını sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Add(Bank bank)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SaveBank(bank);
                return Json(new { Result = "Kayıt işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Banka Düzenlenmesi", "Sistemde seçilen bir banka tanımının düzenlenmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Update(Bank bank)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SaveBank(bank);
                return Json(new { Result = "Güncelleme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Banka Silinmesi", "Sistemde seçilen bir banka tanımının silinmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Delete(Bank bank)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.DestroyBank(bank);
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
