using KonsolideRapor.WebServer.Base;
using Surat.Base.Model.Entities;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Surat.WebServer.Controllers
{
    public class SystemsController : KonsolideControllerBase
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

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit()
        {
            return View();
        }
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
