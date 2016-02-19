using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Surat.Base.Model.Entities;
using Surat.WebServer.Application;
using Surat.WebServer.Base;
using Surat.Common.Data;
namespace Surat.WebServer.Controllers
{
    public class RelationGroupsController: SuratControllerBase
    {
        #region Constructor        
        public RelationGroupsController()
        {
            
        }
        #endregion

        #region Private Members
        
        #endregion

        #region Public Members

        #endregion

        #region Methods        

        [HttpPost]
        public JsonResult UserRoleAdd(RelationGroup relationGroup)
        {
            try
            {
                this.WebApplicationManager.Framework.Security.SaveRelationGroup(relationGroup);
                return Json(new { sonuc = "Kullanıcıya ait rol tanımlaması yapıldı" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {  
                Response.StatusCode = 500;
                return Json(new { sonuc = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }            
        }

        [HttpPost]
        public JsonResult UserWorkgroupAdd(RelationGroup relationGroup)
        {
            try
            {
                this.WebApplicationManager.Framework.Security.SaveRelationGroup(relationGroup);
                return Json(new {sonuc="Kullanıcı çalışma grubuna eklendi" },JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {                
                Response.StatusCode = 500;
                return Json(new { sonuc = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}
