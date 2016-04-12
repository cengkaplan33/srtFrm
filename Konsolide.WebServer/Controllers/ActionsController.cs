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
namespace KonsolideRapor.WebServer.Controllers
{
    public class ActionsController : KonsolideControllerBase
    {
        #region Constructor

        public ActionsController()
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

        public JsonResult GetActions(int pageSize, int skip)
        {
            try
            {
                var Actions = this.WebApplicationManager.Framework.Configuration.GetActiveActionList();
                var total = Actions.Count();
                var data = Actions.OrderBy(m => m.Id).Skip(skip).Take(pageSize).ToList();
                return Json(new { total = total, data = data }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { result = this.PublishException(exception) }, JsonRequestBehavior.AllowGet);
            }
        }

      
        #endregion
    }
}
