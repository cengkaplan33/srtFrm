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
    public class TahsilatTalepController : KonsolideControllerBase
    {
        #region Constructor

        public TahsilatTalepController()
        {

        }

        #endregion

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods

        [ActionAttribute("Tahsilat Talep Sayfası", "Tahsilat talepleri sayfasının görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporWebSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Tahsilat Taleplerinin Çağrılması", "Sistemde kullanılan bütün tahsilat taleplerinin  getirilmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporWebSystemName, Surat.Common.Data.ActionType.Action)]
        public JsonResult GetTahsilatTalepleri(int pageSize, int skip)
        {
            try
            {
                var odemeTalepleri = this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.GetAktifTahsilatTalepleri();
                var total = odemeTalepleri.Count();
                var data = odemeTalepleri.OrderByDescending(m => m.Tarih).Skip(skip).Take(pageSize).ToList();
                return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionAttribute("Tahsilat Talebinin Eklenmesi", "Yeni tahsilat talebinin eklenmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporWebSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Add(OdemeTalep odemeTalep)
        {
            try
            {
                if (odemeTalep.EURO == null)
                    throw new Exception("EURO alanı boş olamaz");
                if (odemeTalep.USD == null)
                    throw new Exception("USD alanı boş olamaz");
                if (odemeTalep.TL == null)
                    throw new Exception("TL alanı boş olamaz");
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SaveTahsilatTalep(odemeTalep);
                return Json(new { Result = "Kayıt işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Tahsilat Talebinin Düzenlenmesi", "Seçilen tahsilat talebinin düzenlenmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporWebSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Update(OdemeTalep odemeTalep)
        {
            try
            {
                if (odemeTalep.EURO == null)
                    throw new Exception("EURO alanı boş olamaz");
                if (odemeTalep.USD == null)
                    throw new Exception("USD alanı boş olamaz");
                if (odemeTalep.TL == null)
                    throw new Exception("TL alanı boş olamaz");
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.SaveTahsilatTalep(odemeTalep);
                return Json(new { Result = "Güncelleme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Tahsilat Talebinin Silinmesi", "Seçilen tahsilat talebinin silinmesini sağlayan metod ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporWebSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
        public JsonResult Delete(OdemeTalep odemeTalep)
        {
            try
            {
                this.WebApplicationManager.KonsolideRapor.KonsolideRaporManager.DestroyOdemeTalep(odemeTalep);
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
