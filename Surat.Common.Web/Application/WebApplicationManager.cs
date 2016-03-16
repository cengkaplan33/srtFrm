using Surat.Base.Exceptions;
using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.Web;
using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Linq;

namespace Surat.WebServer.Application
{
    public class WebApplicationManager : ApplicationManager
    {
        #region Constructor

        public WebApplicationManager():this(null)
        {

        }

        public WebApplicationManager(FrameworkApplicationManager applicationManager)
        {
            if (applicationManager != null)
                this.framework = applicationManager;
            else this.framework = InitializeFramework();            
        }

        #endregion

        #region Private Members

        private WebApplicationContext context;
        private FrameworkApplicationManager framework;  
 
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

        public FrameworkApplicationManager Framework
        {
            get
            {
                return framework;
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

            //Typeof(
            //var sss= new Surat.WebServer.Base.SuratControllerBase();
            //Assembly asm = Assembly.GetExecutingAssembly();
            //Surat.WebServer.Base.SuratControllerBase
            //var ssss = asm.GetTypes()
            //    .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
            //    .SelectMany(type => type.GetMethods())
            //    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));
        }

        #endregion

        #region Methods

        private WebApplicationContext InitializeApplicationContext()
        {
            WebApplicationContext context = new WebApplicationContext(this.Framework.Context);
            
            return context;
        }
 
        private FrameworkApplicationManager InitializeFramework()
        {
            FrameworkApplicationManager framework;
            UserDetailedView currentUser = null;

            //Session da user varsa, ona göre framework başlatılacaktır.

            if (HttpContext.Current != null)
                if (HttpContext.Current.Session["CurrentUser"] != null)
                    currentUser = (UserDetailedView)HttpContext.Current.Session["CurrentUser"];
            
            if (currentUser != null)
                framework = new FrameworkApplicationManager(currentUser);
            else framework = new FrameworkApplicationManager();            
            
            return framework;
        }

        public void Login(string userName, string password)
        {
            UserDetailedView currentUser = null;
            try
            {                
                currentUser = this.Framework.Security.ValidateUser(userName, password);

                if (currentUser != null)
                {
                    this.Framework.Trace.AppendLine(this.Framework.Context.SystemName, "User Validated.", TraceLevel.Basic);
                    this.Framework.StartUserSession(currentUser);
                    //this.Framework.SetCurrentUser(currentUser);
                    this.Context.CurrentUser = currentUser;
                    this.Framework.Trace.AppendLine(this.Framework.Context.SystemName, "User Session started.", TraceLevel.Basic);
                }
                else
                {
                    this.Context.WrongPasswordProcessCount++;
                    if (this.Context.WrongPasswordProcessCount == this.Framework.Context.Security.MaxWrongPasswordAttempts)
                    {
                        this.Framework.Security.SaveUserLock(userName, password, true);
                    }
                    else throw new SecurityException(this.Context.FrameworkContext, "Login", this.Context.SystemId, 
                        String.Format(this.Context.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.Context.FrameworkContext.SystemId,Constants.Message.UserNotAuthorized),userName));
                }

                FormsAuthentication.SetAuthCookie(userName, false);
            }
            catch (Exception exception)
            {

                //PublishException(exception);
                //this.Framework.Exception.Publish(this.Context.FrameworkContext,exception, null);
                throw exception;
            }           
        }

        public void TraceAppendLine(string systemName, string traceInformation, TraceLevel traceLevel)
        {
            if (this.Framework.IsContextInitialized)
                this.Framework.Trace.AppendLine(systemName, traceInformation, traceLevel);
            //else ToDo: Framework üzerinden Trace yazılamaz. Başka bir yöntem takip edilmelidir.
        }

        public string GetGlobalizationKeyValue(int systemId,string globalizationKey)
        {
            if (this.Framework.IsContextInitialized)
                return this.Framework.Globalization.GetGlobalizationKeyValue(systemId,globalizationKey);
            else return globalizationKey;
        }

        public string PublishException(Exception exception)
        {

            if (HttpContext.Current.Request != null)
                this.Framework.Context.ApplicationName = HttpContext.Current.Request.ServerVariables["APPL_MD_PATH"];
            
            if (string.IsNullOrEmpty(this.Framework.Context.ApplicationName))
                this.Framework.Context.ApplicationName = HttpRuntime.AppDomainAppVirtualPath;

            this.Framework.Context.CurrentVariables = new HttpRequestView(HttpContext.Current.Request);
            this.Framework.Context.ApplicationBaseType = "Web";

            UserDetailedView currentUser = null;
            if (this.Framework.IsContextInitialized)
            { 
                if (this.Framework.Context.IsCurrentUserAssigned)
                    currentUser = this.Framework.Context.CurrentUser;
              return  this.Framework.Exception.Publish(this.Context.FrameworkContext, exception, currentUser);
            }
            else  return Constants.Message.FrameworkNotInitialized;
            //else ToDo: Framework üzerinden publish edilemez. Başka bir yöntem takip edilmelidir.
        }

        public void Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                this.Framework.CloseUserSession();
            }
            catch (Exception exception)
            {
                this.Framework.Exception.Publish(this.Context.FrameworkContext, exception, null);
                throw exception;
            }  
        }       

        #endregion

        #region Dispose

        public override void Dispose()
        {
            base.Dispose();
            this.Framework.Dispose();
        }

        #endregion
    }
}

