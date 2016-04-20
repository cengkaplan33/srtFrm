using KonsolideRapor.Business.Application;
using KonsolideRapor.Web;
using Surat.Base.Exceptions;
using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.Web;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Linq;

namespace KonsolideRapor.WebServer.Application
{
    public class WebApplicationManager : ApplicationManager
    {
        #region Constructor

        public WebApplicationManager()
            : this(null)
        {

        }

        public WebApplicationManager(FrameworkApplicationManager frameworkApplicationManager)
        {
            if (frameworkApplicationManager != null)
                this.framework = frameworkApplicationManager;
            else this.framework = InitializeFramework();
            this.konsolideRaporApplicationManager = InitializeKonsolideRapor();
        }

        #endregion

        #region Private Members

        private WebApplicationContext context;
        private FrameworkApplicationManager framework;
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

        public FrameworkApplicationManager Framework
        {
            get
            {
                return framework;
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

            Assembly assembly = Assembly.GetExecutingAssembly();

            ActionsFromReflection = assembly.GetTypes()
                          //.Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                         .Where(type => typeof(KonsolideRapor.WebServer.Base.KonsolideControllerBase).IsAssignableFrom(type))
                         .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                         .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                         .Select(x =>
                         {
                             System.Attribute[] attrs = System.Attribute.GetCustomAttributes(x);
                             Surat.Common.Security.ActionAttribute att = null;
                             foreach (System.Attribute attr in attrs)
                             {
                                 if (attr is Surat.Common.Security.ActionAttribute)
                                 {
                                     att = (Surat.Common.Security.ActionAttribute)attr;
                                     break;
                                 }
                             }

                             if (att == null)
                                 att = new Surat.Common.Security.ActionAttribute("", "", "", ActionType.Action);

                             return new SuratActionView
                             {

                                 Description = att.Description,
                                 Name = att.Name,
                                 SystemName = att.SystemName,
                                 Type = att.Type,
                                 TypeName = x.DeclaringType.Name.Replace("Controller", "") + "/" + x.Name
                             };
                         }).OrderBy(x => x.TypeName).ToList();


        }

        public static List<SuratActionView> ActionsFromReflection;

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

            framework.Security.RegisterActions(ActionsFromReflection);
            ActionsFromReflection = new List<SuratActionView>();

            return framework;
        }
        private KonsolideRaporApplicationManager InitializeKonsolideRapor()
        {
            KonsolideRaporApplicationManager konsolideRaporApplicationManager;

            konsolideRaporApplicationManager = new KonsolideRaporApplicationManager(this.Framework);
            return konsolideRaporApplicationManager;
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
                        String.Format(this.Context.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.Context.FrameworkContext.SystemId, Constants.Message.UserNotAuthorized), userName));
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

        public string GetGlobalizationKeyValue(int systemId, string globalizationKey)
        {
            if (this.Framework.IsContextInitialized)
                return this.Framework.Globalization.GetGlobalizationKeyValue(systemId, globalizationKey);
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
                return this.Framework.Exception.Publish(this.Context.FrameworkContext, exception, currentUser);
            }
            else return Constants.Message.FrameworkNotInitialized;
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

