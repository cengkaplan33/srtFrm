﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.WebServer.Application;
using Surat.WebServer.Base;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using System.Web.Script.Serialization;
namespace Surat.WebServer.Controllers
{
    public class RolesController: SuratControllerBase
    {
        #region Constructor

        public RolesController()
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
        public JsonResult GetRolesByUserName(string filter)
        {
            string b = filter;
            List<SuratRole> suratroles;
            
            try
            {
                suratroles = this.WebApplicationManager.Framework.Security.Role.GetRolesByName(filter);
                return Json(suratroles, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        public JsonResult GetRoles(int pageSize, int skip)
        {
           
            try
            {
                var roles = this.WebApplicationManager.Framework.Security.Role.GetRolesActive();
                var total = roles.Count();
                var data = roles.OrderBy(m => m.Id).Skip(skip).Take(pageSize).ToList();
                return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception exception)
            {               
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        public JsonResult GetRolePages(int? roleId=-1)
        {

            try
            {
               
                return Json( this.WebApplicationManager.Framework.Security.GetUserAccessibleRolePages(roleId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        public JsonResult GetRoleActions(int? roleId = -1)
        {

            try
            {

                return Json(this.WebApplicationManager.Framework.Security.GetRoleAccessibleActions(roleId), JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId, Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Add(SuratRole suratrole,string Pages,string Actions)
        {
            try
            {

                IList<RolePageView> rolePages = new JavaScriptSerializer().Deserialize<IList<RolePageView>>(Pages);
                IList<RoleActionView> roleActions = new JavaScriptSerializer().Deserialize<IList<RoleActionView>>(Actions);
                this.WebApplicationManager.Framework.Security.SaveRole(suratrole);               

                this.WebApplicationManager.Framework.Security.SaveRolePages(suratrole.Id, rolePages);
                this.WebApplicationManager.Framework.Security.SaveRoleActions(suratrole.Id, roleActions);
            
                return Json(new{Result="Kayıt işlemi gerçekleştirildi."}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {    
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Update(SuratRole suratrole,string Pages,string Actions)
        {
            try
            {
                IList<RolePageView> rolePages = new JavaScriptSerializer().Deserialize<IList<RolePageView>>(Pages);
                IList<RoleActionView> roleActions = new JavaScriptSerializer().Deserialize<IList<RoleActionView>>(Actions);
                this.WebApplicationManager.Framework.Security.SaveRolePages(suratrole.Id, rolePages);
                this.WebApplicationManager.Framework.Security.SaveRoleActions(suratrole.Id, roleActions);
                this.WebApplicationManager.Framework.Security.SaveRole(suratrole);
                return Json(new { Result = "Güncelleme işlemi gerçekleştirildi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Delete(SuratRole suratrole)
        {
            try
            {        
                this.WebApplicationManager.Framework.Security.DeleteRole(suratrole);
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
