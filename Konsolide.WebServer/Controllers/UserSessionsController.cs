﻿using Surat.Base.Model.Entities;
using Surat.Common.ViewModel;
using KonsolideRapor.WebServer.Application;
using KonsolideRapor.WebServer.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Surat.Common.Data;
namespace KonsolideRapor.WebServer.Controllers
{
    public class UserSessionsController : KonsolideControllerBase
    {
        #region Constructor

        public UserSessionsController()
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

        public JsonResult GetUserSessions(int pageSize, int skip)
        {
           
            try
            {
                var sessions = this.WebApplicationManager.Framework.Security.GetUserSessionsList();
                var total = sessions.Count();
                var data = sessions.OrderByDescending(m => m.Id).Skip(skip).Take(pageSize).ToList();
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
