using KonsolideRapor.Business.Application;
using KonsolideRapor.Web;
using Surat.Base.Exceptions;
using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.Web;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace KonsolideRapor.WebServer.Application
{
    public class WebApplicationManager : ApplicationManager
    {
        #region Constructor

        public WebApplicationManager():this(null)
        {

        }

        public WebApplicationManager(KonsolideRaporApplicationManager konsolideRaporApplicationManager)
        {
            if (konsolideRaporApplicationManager != null)
                this.konsolideRaporApplicationManager = konsolideRaporApplicationManager;
            else this.konsolideRaporApplicationManager = InitializeKonsolideRapor();
           
            //else this.konsolideRapor=InitializeApplicationContext
        }

        #endregion

        #region Private Members

        private WebApplicationContext context;

        private KonsolideRaporApplicationManager konsolideRaporApplicationManager;
        #endregion

        #region Public Members

        public WebApplicationContext Context
        {
            get
            {
                if (context == null)
                    context = InitializeApplicationContext();

                return context;
            }
        }

      

        public KonsolideRaporApplicationManager KonsolideRapor
        {
            get
            {
                return konsolideRaporApplicationManager;
            }
        }
     
        #endregion

        #region Static Methods

        public static void InitializeMVCApplication()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfiguration.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfiguration.RegisterRoutes(RouteTable.Routes);
            BundleConfiguration.RegisterBundles(BundleTable.Bundles); 
        }

        #endregion

        #region Methods

        private WebApplicationContext InitializeApplicationContext()
        {
            WebApplicationContext context = new WebApplicationContext(this.KonsolideRapor.Context);
            
            return context;
        }
 
      

        private KonsolideRaporApplicationManager InitializeKonsolideRapor()
        {
            KonsolideRaporApplicationManager konsolideRapor;
            UserDetailedView currentUser = null;

            //Session da user varsa, ona göre framework başlatılacaktır.

            if (HttpContext.Current != null)
                if (HttpContext.Current.Session["CurrentUser"] != null)
                    currentUser = (UserDetailedView)HttpContext.Current.Session["CurrentUser"];

            if (currentUser != null)
                konsolideRapor = new KonsolideRaporApplicationManager();
            else konsolideRapor = new KonsolideRaporApplicationManager();

            return konsolideRapor;
        }

        public void Login(string userName, string password)
        {
            UserDetailedView currentUser = null;
            try
            {                
                currentUser = this.KonsolideRapor.Framework.Security.ValidateUser(userName, password);

                if (currentUser != null)
                {
                    this.KonsolideRapor.Framework.Trace.AppendLine(this.KonsolideRapor.Framework.Context.SystemName, "User Validated.", TraceLevel.Basic);
                    this.KonsolideRapor.Framework.StartUserSession(currentUser);
                    this.Context.CurrentUser = currentUser;
                    this.KonsolideRapor.Framework.Trace.AppendLine(this.KonsolideRapor.Framework.Context.SystemName, "User Session started.", TraceLevel.Basic);
                }
                else
                {
                    this.Context.WrongPasswordProcessCount++;
                    if (this.Context.WrongPasswordProcessCount == this.KonsolideRapor.Framework.Context.Security.MaxWrongPasswordAttempts)
                    {
                        this.KonsolideRapor.Framework.Security.SaveUserLock(userName, password, true);
                    }
                    else throw new SecurityException(this.Context.FrameworkContext, "Login", this.Context.SystemId, 
                        String.Format(this.Context.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.Context.FrameworkContext.SystemId,Constants.Message.UserNotAuthorized),userName));
                }

                FormsAuthentication.SetAuthCookie(userName, false);
            }
            catch (Exception exception)
            {

                //PublishException(exception);
                //this.KonsolideRapor.Framework.Exception.Publish(this.Context.FrameworkContext,exception, null);
                throw exception;
            }           
        }

        public void TraceAppendLine(string systemName, string traceInformation, TraceLevel traceLevel)
        {
            if (this.KonsolideRapor.Framework.IsContextInitialized)
                this.KonsolideRapor.Framework.Trace.AppendLine(systemName, traceInformation, traceLevel);
            //else ToDo: Framework üzerinden Trace yazılamaz. Başka bir yöntem takip edilmelidir.
        }

        public string GetGlobalizationKeyValue(int systemId,string globalizationKey)
        {
            if (this.KonsolideRapor.Framework.IsContextInitialized)
                return this.KonsolideRapor.Framework.Globalization.GetGlobalizationKeyValue(systemId,globalizationKey);
            else return globalizationKey;
        }

        public string PublishException(Exception exception)
        {

            if (HttpContext.Current.Request != null)
                this.KonsolideRapor.Framework.Context.ApplicationName = HttpContext.Current.Request.ServerVariables["APPL_MD_PATH"];
            
            if (string.IsNullOrEmpty(this.KonsolideRapor.Framework.Context.ApplicationName))
                this.KonsolideRapor.Framework.Context.ApplicationName = HttpRuntime.AppDomainAppVirtualPath;

            this.KonsolideRapor.Framework.Context.CurrentVariables = new HttpRequestView(HttpContext.Current.Request);
            this.KonsolideRapor.Framework.Context.ApplicationBaseType = "Web";

            UserDetailedView currentUser = null;
            if (this.KonsolideRapor.Framework.IsContextInitialized)
            { 
                if (this.KonsolideRapor.Framework.Context.IsCurrentUserAssigned)
                    currentUser = this.KonsolideRapor.Framework.Context.CurrentUser;
              return  this.KonsolideRapor.Framework.Exception.Publish(this.Context.FrameworkContext, exception, currentUser);
            }
            else  return Constants.Message.FrameworkNotInitialized;
            //else ToDo: Framework üzerinden publish edilemez. Başka bir yöntem takip edilmelidir.
        }

        public void Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                this.KonsolideRapor.Framework.CloseUserSession();
            }
            catch (Exception exception)
            {
                this.KonsolideRapor.Framework.Exception.Publish(this.Context.FrameworkContext, exception, null);
                throw exception;
            }  
        }       

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
            this.KonsolideRapor.Framework.Dispose();
        }

        #endregion
    }
}

