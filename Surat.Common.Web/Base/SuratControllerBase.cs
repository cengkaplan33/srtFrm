using Surat.Base.Application;
using Surat.Common.Data;
using Surat.SerendipApplication.Business;
using Surat.WebServer.ActionFilters;
using Surat.WebServer.Application;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Surat.WebServer.Base
{
    [SuratAuthorizationFilter][LogActionFilter]
    public class SuratControllerBase : Controller,IDisposable
    {
        #region Constructor

        public SuratControllerBase()
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = 
                    CultureInfo.GetCultureInfo(this.WebApplicationManager.Framework.Globalization.GetCurrentCultureName());
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
        private SerendipApplicationManager serendip;
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

        public SerendipApplicationManager Serendip
        {
            get
            {
                if (serendip == null)
                    serendip = new SerendipApplicationManager(this.WebApplicationManager.Framework);

                return serendip;
            }
        }

        #endregion

        #region Methods

        public string PublishException(Exception exception)
        {       
            if (this.WebApplicationManager.Framework.IsContextInitialized)
                this.exceptionMessage = this.WebApplicationManager.Framework.Exception.Publish(this.WebApplicationManager.Framework.Context, exception, this.WebApplicationManager.Context.CurrentUser);
            else this.exceptionMessage = Constants.Message.FrameworkNotInitialized;
            //else  To Do : Framework initialize olmadığı durumda, Exception publish edilemez. Ele alınmalıdır.

            return this.exceptionMessage;
        }        

        #endregion

        #region Dispose

        void IDisposable.Dispose()
        {
            try
            {
                this.Serendip.Dispose();
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
