﻿using Surat.Base.Model.Entities;
using Surat.Common.Data;
using Surat.WebServer.Application;
using Surat.WebServer.Base;
using System;
using Surat.Common.Security;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace Surat.WebServer.Controllers
{
    public  class SystemsController:SuratControllerBase
    {
        #region Constructor

        public SystemsController()
        {

        }
        #endregion

        #region Private Members
       
        #endregion              

        #region Public Members

        #endregion

        #region Methods

        [ActionAttribute("Sistem Sayfası", "Sayfanın görüntülenmesini sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Sistem Düzenleme Sayfası", "Sayfanın görüntülenmesini sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
        public ActionResult Edit()
        {
            return View();
        }

        [ActionAttribute("Sistemleri Getir", "Sistemde kayıtlı olan tüm aktif sistemleri getirir.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult GetSystems()
        {           
            try
            {
                var suratsystems = this.WebApplicationManager.Framework.Configuration.System.GetActiveSystems();
                return Json(suratsystems, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {      
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        [ActionAttribute("Sistem Ekle", "Yeni sistem ekler", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult Add(SuratSystem suratsystem)
        {

            try
            {
                this.WebApplicationManager.Framework.Configuration.SaveSystem(suratsystem);
                return Json(new { Result = "Kayıt işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
            
        }

        [HttpPost]
        [ActionAttribute("Sistem Güncelle", "Seçilen sistemi günceller.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult Update(SuratSystem suratsystem)
        {
            try
            {
                this.WebApplicationManager.Framework.Configuration.SaveSystem(suratsystem);
                return Json(new {Result="Güncelleme işlemi gerçekleştirildi."}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        [ActionAttribute("Sistem Sil", "Seçilen sistemi siler.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
        public JsonResult Delete(SuratSystem suratsystem)
        {
            try
            {
                this.WebApplicationManager.Framework.Configuration.DeleteSystem(suratsystem);
                return Json(new {Result="Silme işlemi gerçekleştirildi."}, JsonRequestBehavior.AllowGet);
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
