using Surat.Base.Model.Entities;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.Common.Security;
using Surat.WebServer.Application;
using Surat.WebServer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Surat.WebServer.Controllers
{
    public class PagesController : SuratControllerBase
    {
        #region Constructor

        public PagesController()
        {
            
        }

        #endregion

        #region Private Members

        
        #endregion

        #region Public Members

        #endregion

        #region Methods

        [ActionAttribute("Sayfalar Sayfası", "Sayfanın görüntülenmesini sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Sayfa Düzenleme Sayfası", "Sayfanın görüntülenmesini sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
        public ActionResult Edit()
        {
            return View();
        }

        [ActionAttribute("Sayfaları Sisteme Göre Getir", "Seçilen sisteme ait sayfaları getirir.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult GetPagesBySystem(object[] parent)
        {
            int systemId;
            List<Page> pages;

            try
            {
                systemId = int.Parse(parent[0].ToString());
            }
            catch
            {
                systemId = 0;
            }

            try
            {
                pages = this.WebApplicationManager.Framework.Configuration.Page.GetSystemPages(systemId);

                int total = pages.Count;
                return Json(new { data = pages, total = total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {                
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }            
        }

        [ActionAttribute("Sayfaları Parametrelere Göre Getir", "Kullanıcı, rol, çalışma grubu parametrelerine göre sayfaları getirir.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult GetSystemPagesByParameters(int userId,int roleId,int workgroupId)
        {
            List<RelationGroupAccessiblePageView> pages;
           
            try
            {
                pages = this.WebApplicationManager.Framework.Security.GetAccessiblePages(userId, roleId, workgroupId);

                int total = pages.Count;
                return Json(new { data = pages, total = total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Sayfaları Getir", "Sistemde kayıtlı olan tüm aktif sayfaları getirir.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]     
        public JsonResult GetPages(int pageSize, int skip)
        {
            try
            {
                var Pages = this.WebApplicationManager.Framework.Configuration.GetActivePageList();
                var total = Pages.Count();
                var data = Pages.OrderBy(m => m.Id).Skip(skip).Take(pageSize).ToList();
                return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ActionAttribute("Sayfa Ekle", "Sisteme yeni sayfa ekler", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult Add(Page page)
        {
            try
            {
                this.WebApplicationManager.Framework.Configuration.SavePage(page);
                return Json(new { Result = "Kayıt işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        [ActionAttribute("Sayfa Güncelle", "Seçilen sayfayı günceller.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult Update(Page page)
        {
            try
            {
                this.WebApplicationManager.Framework.Configuration.SavePage(page);
                return Json(new { Result = "Güncelleme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {                
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        [ActionAttribute("Sayfa Sil", "Seçilen sayfayı siler.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult Delete(Page page)
        {

            try
            {
                this.WebApplicationManager.Framework.Configuration.DeletePage(page);
                return Json(new { Result = "Silme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {                
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }

        }
      
        #endregion
    }
}
