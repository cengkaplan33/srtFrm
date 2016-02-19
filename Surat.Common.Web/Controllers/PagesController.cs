using Surat.Base.Model.Entities;
using Surat.Common.Data;
using Surat.Common.ViewModel;
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

        public ActionResult Index()
        {
            return View();
        }
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

        [HttpPost]
        public JsonResult Add([Bind(Prefix = "pages")]IEnumerable<Page> pages, object[] parent)
        {
            try
            {
                pages.Last().SystemId = int.Parse(parent[0].ToString());
            }
            catch
            {
                pages.Last().SystemId = 0;
            }

            try
            {
                this.WebApplicationManager.Framework.Configuration.SavePage(pages.Last());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {                
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }            
        }

        [HttpPost]
        public JsonResult Update([Bind(Prefix = "pages")]IEnumerable<Page> pages, object[] parent)
        {
            try
            {
                this.WebApplicationManager.Framework.Configuration.SavePage(pages.Last());
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {                
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Delete([Bind(Prefix = "pages")]IEnumerable<Page> pages)
        {

            try
            {
                this.WebApplicationManager.Framework.Configuration.DeletePage(pages.Last());
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
