﻿using KonsolideRapor.WebServer.Base;
using Surat.Base.Model.Entities;
using Surat.Common.Data;
using Surat.Common.Security;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Surat.WebServer.Controllers
{
    public class PagesController : KonsolideControllerBase
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

        [ActionAttribute("Sistem Sayfaları", "Sistem sayfalarının görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Sistem Sayfaları Düzenleme", "Sistem sayfalarının düzenlenmesini sağlayan sayfanın görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Page)]
        public ActionResult Edit()
        {
            return View();
        }

        [ActionAttribute("Belirili Bir Sisteme Ait Sayfalar", "Belirli bir sisteme ait sayfaların görüntülenmesini sağlar ", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
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
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Parametreler ile Sayfaların Getirilmesi", "Sisteme ait sayfaların belirli parametreler ile getirilmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        public JsonResult GetSystemPagesByParameters(int userId, int roleId, int workgroupId)
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
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Sistem Sayfaların Getirilmesi", "Sisteme ait sayfaların getirilmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
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

        [ActionAttribute("Sayfa Eklenmesi", "Sisteme ait yeni bir sayfa eklenmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
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

        [ActionAttribute("Sayfa Düzenlenmesi", "Seçilen bir  sistem sayfasının düzenlenmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
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
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [ActionAttribute("Sayfa Silinmesi", "Seçilen bir  sistem sayfasının silinmesini sağlayan metod", KonsolideRapor.Common.Data.KonsolideRaporConstants.Application.KonsolideRaporSystemName, Surat.Common.Data.ActionType.Action)]
        [HttpPost]
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
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }

        }

        #endregion
    }
}
