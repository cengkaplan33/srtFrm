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
    public class ExceptionsController : KonsolideControllerBase
    {
        #region Constructor

        public ExceptionsController()
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

        public JsonResult GetExceptions(int pageSize, int skip)
        {
           
            try
            {
                var exceptions = this.WebApplicationManager.Framework.Exception.GetExceptionsList();
                var total = exceptions.Count();
                var data = exceptions.OrderByDescending(m => m.Id).Skip(skip).Take(pageSize).ToList();
                return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);
               
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
