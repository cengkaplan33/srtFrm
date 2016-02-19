using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.WebService.Application;
using Surat.WebService.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Surat.WebService.Application
{
    public class ServiceApplicationManager : ApplicationManager
    {
        #region Constructor

        public ServiceApplicationManager(UserDetailedView currentServiceUser)
            : this(currentServiceUser,null)
        {

        }

        public ServiceApplicationManager(UserDetailedView currentServiceUser,FrameworkApplicationManager applicationManager)
        {
            if (applicationManager != null)  
            {
                this.framework = applicationManager;
                if (currentServiceUser != null)
                    this.framework.SetCurrentUser(currentServiceUser);
            }
            else this.framework = InitializeFramework(currentServiceUser);  
        }

        #endregion

        #region Private Members

        private ServiceApplicationContext context;
        private FrameworkApplicationManager framework;
        private AuthenticationManager authenticationManager;
 
        #endregion

        #region Public Members

        public ServiceApplicationContext Context
        {
            get
            {
                if (context == null)
                    context = InitializeApplicationContext();

                return context;
            }
        }

        public FrameworkApplicationManager Framework
        {
            get
            {
                return framework;
            }
        }

        public AuthenticationManager Authentication
        {
            get
            {
                if (authenticationManager == null)
                    InitializeAuthenticationManager();

                return authenticationManager;
            }
        }

        #endregion 

        #region Methods

        private ServiceApplicationContext InitializeApplicationContext()
        {
            ServiceApplicationContext context = new ServiceApplicationContext(this.Framework.Context);            

            return context;
        }

        private FrameworkApplicationManager InitializeFramework(UserDetailedView currentUser)
        {
            FrameworkApplicationManager framework;

            if (currentUser != null)
                framework = new FrameworkApplicationManager(currentUser);
            else framework = new FrameworkApplicationManager();
            
            return framework;
        }

        private void InitializeAuthenticationManager()
        {
            authenticationManager = new AuthenticationManager(this.Framework);
            this.Framework.Trace.AppendLine(this.Context.SystemName, "AuthenticationManager Initialized.", TraceLevel.Basic);
        }

        public void TraceAppendLine(string systemName, string traceInformation, TraceLevel traceLevel)
        {
            if (this.Framework.IsContextInitialized)
                this.Framework.Trace.AppendLine(systemName, traceInformation, traceLevel);
            //else ToDo: Framework üzerinden Trace yazılamaz. Başka bir yöntem takip edilmelidir.
        }

        public string PublishException(Exception exception)
        {
            UserDetailedView currentUser = null;
            string message = string.Empty;
            
            if (this.Framework.Context.IsCurrentUserAssigned)
                currentUser = this.Framework.Context.CurrentUser;

            if (this.Framework.IsContextInitialized)
                message = this.Framework.Exception.Publish(this.Framework.Context, exception, currentUser);
            //else ToDo: Framework üzerinden publish edilemez. Başka bir yöntem takip edilmelidir.

            return message;
        }

        public string GetGlobalizationKeyValue(int systemId, string globalizationKey)
        {
            if (this.Framework.IsContextInitialized)
                return this.Framework.Globalization.GetGlobalizationKeyValue(systemId, globalizationKey);
            else return globalizationKey;
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            this.Framework.Dispose();
        }

        #endregion
    }
}

