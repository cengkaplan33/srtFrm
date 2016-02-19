using Surat.Base.Model.Entities;
using Surat.Common.Data;
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
    public class ParametersController: SuratControllerBase
    {
        #region Constructor

        public ParametersController()
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
       
        public JsonResult GetParametersBySystem(object[] parent)
        {
            int dbObjectId;
            List<Parameter> parameters;

            try
            {
                dbObjectId = int.Parse(parent[0].ToString());
            }
            catch
            {
                dbObjectId = 0;
            }

            try
            {
                parameters = this.WebApplicationManager.Framework.Configuration.Parameter.GetParametersBySystemId(dbObjectId);
                return Json(new { data = parameters, total = parameters.Count }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {              
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Add([Bind(Prefix = "parameters")]IEnumerable<Parameter> parameter, object[] parent)
        {
            try
            {
                parameter.Last().DBObjectId = int.Parse(parent[0].ToString());
            }
            catch
            {
                parameter.Last().DBObjectId = 0;
            }

            try
            {
                this.WebApplicationManager.Framework.Configuration.SaveParameter(parameter.Last());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {  
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }            
        }

        [HttpPost]
        public JsonResult Update([Bind(Prefix = "parameters")]IEnumerable<Parameter> parameters,object[] parent)
        {
            try
            {
                this.WebApplicationManager.Framework.Configuration.SaveParameter(parameters.Last());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {               
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Delete([Bind(Prefix = "parameters")]IEnumerable<Parameter> parameters)
        {
            try
            {
                this.WebApplicationManager.Framework.Configuration.DeleteParameter(parameters.Last());
                return Json("", JsonRequestBehavior.AllowGet);
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
