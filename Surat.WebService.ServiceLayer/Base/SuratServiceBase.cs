using Surat.Base.Application;
using Surat.Business.Application;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.SerendipApplication.Business;
using Surat.WebService.Application;
using Surat.WebService.Common.Data;
using Surat.WebServiceBase.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Surat.WebService.ServiceLayer
{
    public class SuratServiceBase : ApiController,IDisposable
    {
        #region Constructor

        public SuratServiceBase()
        {
            //try
            //{
                             
            //}
            //catch (Exception exception)
            //{
            //    this.PublishException(exception);
            //}             
        }
        
        #endregion

        #region Private Members

        private FrameworkApplicationManager framework;
        private ServiceApplicationManager serviceApplicationManager;
        private SerendipApplicationManager serendip;
        private string exceptionMessage;
        private UserDetailedView currentServiceUser;
 
        #endregion

        #region Public Members

        public FrameworkApplicationManager Framework
        {
            get
            {
                if (framework == null)
                    framework = InitializeFramework();

                return framework;
            }
        }

        public UserDetailedView CurrentServiceUser
        {
            get 
            {
                if (currentServiceUser == null)
                    currentServiceUser = GetCurrentServiceUser();
                return currentServiceUser; 
            }
        }

        public string ExceptionMessage 
        {
            get { return exceptionMessage; } 
        }

        public ServiceApplicationManager ServiceApplicationManager
        {
            get
            {
                if (serviceApplicationManager == null)
                    InitializeServiceApplicationManager();
                    

                return serviceApplicationManager;
            }
        }

        public SerendipApplicationManager Serendip
        {
            get
            {
                if (serendip == null)
                    serendip = new SerendipApplicationManager(this.ServiceApplicationManager.Framework);

                return serendip;
            }
        }

        #endregion

        #region Methods 
    
        private FrameworkApplicationManager InitializeFramework()
        {
            FrameworkApplicationManager framework;

            framework = new FrameworkApplicationManager();

            return framework;
        }

        private void InitializeServiceApplicationManager()
        {           
            serviceApplicationManager = new ServiceApplicationManager(this.CurrentServiceUser,this.Framework);
            InitializeCulture();//ServiceAppManager a erişim var. Request alınamıyor. REquest içindeki değer, ServiceAppManager için gereklidir.
        }
   
        private void InitializeCulture()
        {
            Thread.CurrentThread.CurrentCulture =
                    CultureInfo.GetCultureInfo(this.ServiceApplicationManager.Framework.Globalization.GetCurrentCultureName());
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        private AuthenticationToken ProcessHttpRequest()
        {
            object tokenObject = null;
            AuthenticationToken authenticationToken = null;

            this.Request.Properties.TryGetValue(ServiceConstants.Application.AuthenticationToken, out tokenObject);
            authenticationToken = (AuthenticationToken)tokenObject;

            return authenticationToken;
        }

        private UserDetailedView GetCurrentServiceUser()
        {
            AuthenticationToken authenticationToken;            
            UserDetailedView currentServiceUser = null;

            if (!this.Request.RequestUri.AbsolutePath.Contains(ServiceConstants.Application.AuthenticationRequest))
            {
                authenticationToken = ProcessHttpRequest();               
                currentServiceUser = this.Framework.Security.ValidateUser(authenticationToken.UserName, authenticationToken.Password);
            }

            return currentServiceUser;
        }

        public string PublishException(Exception exception)
        {       
            if (this.ServiceApplicationManager.Framework.IsContextInitialized)
                this.exceptionMessage = this.ServiceApplicationManager.Framework.Exception.Publish(this.ServiceApplicationManager.Framework.Context, exception, this.ServiceApplicationManager.Framework.Context.CurrentUser);
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
                this.ServiceApplicationManager.Dispose();
                base.Dispose();
            }
            catch
            {
               //ToDo : Ele alınmalıdır.                
            }            
        }

        #endregion
    }
}
