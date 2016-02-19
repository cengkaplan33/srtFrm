using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.WebServer.Application;
using System.Web;
using Surat.WebServer.Base;
using Surat.Common.Data;

namespace Surat.WebServer.Controllers
{
  
    public class WorkgroupsController:SuratControllerBase
    {        
        #region Constructor
        public WorkgroupsController()
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
        public JsonResult GetWorkgroups()
        {   
            try
            {

             var  workgroups = this.WebApplicationManager.Framework.Security.Workgroup.GetActiveWorkGroups();
               
                return Json(workgroups, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception exception)
            {
                this.PublishException(exception);
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + exception.Message });
            }
        }

        [HttpPost]
        public JsonResult Add(Workgroup workgroup)
        {          
            try
            {
                this.WebApplicationManager.Framework.Security.SaveWorkgroup(workgroup);
                return Json(new {Result="Kayıt işlemi gerçekleştirildi."}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                this.PublishException(exception);
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + exception.Message });
            }
        }

        [HttpPost]
        public JsonResult Update(Workgroup workgroup)
        {
            try
            {                
                this.WebApplicationManager.Framework.Security.SaveWorkgroup(workgroup);
                return Json(new { Result = "Kaydınız güncelleştirildi" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Delete(Workgroup workgroup)
        {
            try
            {
                this.WebApplicationManager.Framework.Security.DeleteWorkgroup(workgroup);
                return Json(new { Result = "Kaydınız silindi" }, JsonRequestBehavior.AllowGet);
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
