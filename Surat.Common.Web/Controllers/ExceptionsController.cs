﻿using System;
using System.Collections.Generic;
using Surat.Common.Security;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.WebServer.Application;
using Surat.WebServer.Base;
using Surat.Common.Data;
namespace Surat.WebServer.Controllers
{
    public class ExceptionsController: SuratControllerBase
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

        [ActionAttribute("Hata Sayfası", "Sayfanın görüntülenmesini sağlar.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Page)]
        public ActionResult Index()
        {
            return View();
        }

        [ActionAttribute("Hataları Getir", "Sistemde loglanan tüm hataları getirir.", Surat.Common.Data.Constants.Application.WebFrameworkSystemName, ActionType.Action)]
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
