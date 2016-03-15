using Surat.Common.Data;
using Surat.Common.ViewModel;
using KonsolideRapor.WebServer.ActionFilters;
using KonsolideRapor.WebServer.Application;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace KonsolideRapor.WebServer.Base
{
    [KonsolideAuthorizationFilter]
    [LogActionFilter]
    public class KonsolideControllerBase : Controller, IDisposable
    {
        #region Constructor

        public KonsolideControllerBase()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture =
                    CultureInfo.GetCultureInfo(this.WebApplicationManager.KonsolideRapor.Framework.Globalization.GetCurrentCultureName());
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            }
            catch (Exception exception)
            {
                this.PublishException(exception);
            }
        }

        #endregion

        #region Private Members

        private WebApplicationManager webApplicationManager;
        private string exceptionMessage;

        #endregion

        #region Public Members

        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }

        public WebApplicationManager WebApplicationManager
        {
            get
            {
                if (webApplicationManager == null)
                    webApplicationManager = new WebApplicationManager();

                return webApplicationManager;
            }
        }

      

        #endregion

        #region Methods

        public string PublishException(Exception exception)
        {
            this.exceptionMessage = this.WebApplicationManager.PublishException(exception);

            //if (Request != null)
            //    this.WebApplicationManager.KonsolideRapor.Framework.Context.ApplicationName =Request.ServerVariables["APPL_MD_PATH"];

            //if (string.IsNullOrEmpty(this.WebApplicationManager.KonsolideRapor.Framework.Context.ApplicationName))
            //    this.WebApplicationManager.KonsolideRapor.Framework.Context.ApplicationName = HttpRuntime.AppDomainAppVirtualPath;

            //this.WebApplicationManager.KonsolideRapor.Framework.Context.ApplicationBaseType = "Web";
            //this.WebApplicationManager.KonsolideRapor.Framework.Context.MachineName= "Web";

            //if (this.WebApplicationManager.KonsolideRapor.Framework.IsContextInitialized)
            //    this.exceptionMessage = this.WebApplicationManager.KonsolideRapor.Framework.Exception.Publish(this.WebApplicationManager.KonsolideRapor.Framework.Context, exception, this.WebApplicationManager.Context.CurrentUser);
            //else this.exceptionMessage = Constants.Message.FrameworkNotInitialized;
            ////else  To Do : Framework initialize olmadığı durumda, Exception publish edilemez. Ele alınmalıdır.

            return this.exceptionMessage;
        }

        #endregion

        #region Dispose

        void IDisposable.Dispose()
        {
            try
            {
             
                this.WebApplicationManager.Dispose();
                base.Dispose();
            }
            catch
            {
                //ToDo : Ele alınmalıdır.                
            }

        }

        #endregion

        #region Override

        protected override void HandleUnknownAction(string actionName)
        {
            ViewBag.ErrorMessage = "İşlem bulunamadı";

            this.View("Error").ExecuteResult(this.ControllerContext);
        }

        #endregion
    }
}