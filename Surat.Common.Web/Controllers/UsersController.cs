using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
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
    public class UsersController : SuratControllerBase
    {
        #region Constructor

        public UsersController()
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
        public ActionResult GetUsers(int pageSize,int skip)
        {
            
            try
            {
                var users = this.WebApplicationManager.Framework.Security.User.GetUsersActive();
               var total = users.Count();
               var data =users.OrderBy(m=>m.Id).Skip(skip).Take(pageSize).ToList();
               return Json(new { total = total, data = data },JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {            
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public ActionResult Add(SuratUser user)
        {
            try
            {
             
                    this.WebApplicationManager.Framework.Security.SaveUser(user);

                    return Json(new { Result = "Kullanıcı başarılı bir şekilde oluşturuldu." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {           
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Update(SuratUser user)
        {
            try
            {

                this.WebApplicationManager.Framework.Security.SaveUser(user);

                return Json(new { Result = "Kullanıcı bilgileri başarılı bir şekilde güncellendi." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Response.StatusCode = 500;
                return Json(new { Result = this.WebApplicationManager.GetGlobalizationKeyValue(this.WebApplicationManager.Framework.Context.SystemId,Constants.Message.OperationNotCompleted) + " " + this.PublishException(exception) });
            }
        }

        [HttpPost]
        public JsonResult Delete(SuratUser users)
        {
            try
            {
               
                this.WebApplicationManager.Framework.Security.DeleteUser(users);

                return Json(new {Result="Kayıt başarılı bir şekilde silindi"},JsonRequestBehavior.AllowGet);
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
