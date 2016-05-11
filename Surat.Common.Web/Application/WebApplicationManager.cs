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
using System.Collections.Generic;
using Surat.Base.Model.Entities;

namespace Surat.WebServer.Application
{
    public class WebApplicationManager : ApplicationManager
    {
        #region Constructor

        public WebApplicationManager()
            : this(null)
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
        private SuratRights rights = null;

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

        public SuratRights Rights
        {
            get
            {
                if (rights == null)
                    InitializeRights();

                return rights;
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

            //    var controlleractionlist = assembly.GetTypes()
            //.Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
            //.SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            //.Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            //.Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
            //.OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            //    Actions = controlleractionlist.Select(x => 
            //        {
            //            if ( Math.Round(2.4)  == 2)
            //                return new SuratActionView { TypeName = x.Controller.Replace("Controller", "") + "/" + x.Action };
            //            else
            //          return      new SuratActionView { TypeName = x.Controller.Replace("Controller", "") + "/" + x.Action };
            //        } 
            //        ).ToList();
            //    Surat.Common.Security.ActionAttribute ss = (Surat.Common.Security.ActionAttribute)assembly;


            ////linqpad
            //assembly.GetTypes()
            //        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
            //    //.Where(type=> typeof(Surat.WebServer.Base.SuratControllerBase).IsAssignableFrom(type))
            //        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            //        .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            //        .Select(x =>
            //        {
            //            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(x);
            //            Surat.Common.Security.ActionAttribute att = null;
            //            foreach (System.Attribute attr in attrs)
            //            {
            //                if (attr is Surat.Common.Security.ActionAttribute)
            //                {
            //                    att = (Surat.Common.Security.ActionAttribute)attr;
            //                    break;
            //                }
            //            }

            //            if (att == null)
            //                att = new Surat.Common.Security.ActionAttribute("", "", "");
            //            return new { def = att, URL = x.DeclaringType.Name.Replace("Controller", "") + "/" + x.Name, Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) };
            //        }).OrderBy(x => x.URL).ThenBy(x => x.Action).ToList();


            ActionsFromReflection = assembly.GetTypes()
                //.Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                         .Where(type => typeof(Surat.WebServer.Base.SuratControllerBase).IsAssignableFrom(type))
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

            //Typeof(
            //var sss= new Surat.WebServer.Base.SuratControllerBase();
            //Assembly asm = Assembly.GetExecutingAssembly();
            //Surat.WebServer.Base.SuratControllerBase
            //var ssss = asm.GetTypes()
            //    .Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
            //    .SelectMany(type => type.GetMethods())
            //    .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));
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


            #region OK::NOT:: aslında session nesnesine gerek yok zaten FormsAuthentication yaptık ve kullanıcı adı ile login olduk daha sonra session'ı kontrol etsek daha mantıklı ve düzgün olacak ki zaten çoğu yerde IsAuthenticated kontrolü yapılmış.
            //if (HttpContext.Current.Request.IsAuthenticated)
            //{
            //    var uss = HttpContext.Current.User.Identity;

            //    if (HttpContext.Current != null)
            //        if (HttpContext.Current.Session["CurrentUser"] != null)
            //            currentUser = (UserDetailedView)HttpContext.Current.Session["CurrentUser"];
            //}
            #endregion

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

        private void InitializeRights()
        {
            rights = new SuratRights();
            rights.WebAuditor = this.framework.Security.RegisterRight("WebAuditor", "WebAuditor hakkının açıklamasını buraya girelim inş.  :)  ", this.Context.SystemId);
            rights.WebAuditManagement = this.framework.Security.RegisterRight("WebAuditManagement", "WebAuditManagement hakkının açıklamasını buraya girelim inş.  :)  ", this.Context.SystemId);
        }



        public void Login(string userName, string password, bool isActiveDirectoryUser)
        {
            UserDetailedView currentUser = null;
            SuratUser user = null;
            try
            {
                if (isActiveDirectoryUser)
                {

                    user = this.Framework.Security.User.GetActiveDirectoryUser(Environment.UserName);
                    if (user == null)
                        throw new SecurityException(this.Context.FrameworkContext, "Login", this.Context.SystemId,
                       String.Format(this.Context.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.Context.FrameworkContext.SystemId, Constants.Message.UserNotAuthorized), userName));
                    if (!this.Framework.ActiveDirectory.ActiveDirectoryUserCheck())
                        throw new SecurityException(this.Context.FrameworkContext, "ActiveDirectoryLogin", this.Context.SystemId,
                              String.Format(this.Context.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.Context.FrameworkContext.SystemId, Constants.Message.ActiveDirectoryUserNotAuthorized), userName));
                    currentUser = this.Framework.Security.ValidateUser(user.UserName, user.Password);
                }
                else
                {
                    currentUser = this.Framework.Security.ValidateUser(userName, password);
                }

                if (currentUser != null)
                {
                    this.Framework.Trace.AppendLine(this.Framework.Context.SystemName, "User Validated.", TraceLevel.Basic);
                    this.Framework.StartUserSession(currentUser);
                    //this.Framework.SetCurrentUser(currentUser);
                    this.Context.CurrentUser = currentUser;
                    this.Framework.Trace.AppendLine(this.Framework.Context.SystemName, "User Session started.", TraceLevel.Basic);


                    #region OK::NOT:: Login FormsAuthentication işleminin olmasının sağlayan 2 method. birinci method'ta bazı değerleri değiştirebilme şansımız var. şimdilik ikiyi kullanacağız.
                    /*
                     * FormsAuthentication sonucunda HttpContext.Current.Request.IsAuthenticated ve HttpContext.Current.User.Identity alanları dolmuş olacak session'ı kontrol etmemize gerek kalmaz.
                     */
                    ////1. method
                    //HttpCookie authCookie = FormsAuthentication.GetAuthCookie(userName, true);
                    //FormsAuthenticationTicket tempTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    //var cookiePath = HttpContext.Current.Request.ApplicationPath;
                    //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    //    tempTicket.Version,
                    //    tempTicket.Name,
                    //    tempTicket.IssueDate,
                    //    tempTicket.Expiration,
                    //    true,
                    //    "",
                    //    cookiePath);
                    //authCookie.Value = FormsAuthentication.Encrypt(authTicket);
                    //authCookie.Name = FormsAuthentication.FormsCookieName;
                    //authCookie.Path = cookiePath;

                    //HttpContext.Current.Response.Cookies.Remove(authCookie.Name);
                    //HttpContext.Current.Response.Cookies.Add(authCookie);

                    ////2. method
                    FormsAuthentication.SetAuthCookie(userName, false);

                    #endregion
                }
                else
                {
                    throw new SecurityException(this.Context.FrameworkContext, "Login", this.Context.SystemId,
                       String.Format(this.Context.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.Context.FrameworkContext.SystemId, Constants.Message.UserNotAuthorized), userName));
                }
            }

            catch (WrongPasswordException)
            {
                this.Context.WrongPasswordProcessCount++;

                if (this.Context.WrongPasswordProcessCount == this.Framework.Context.Security.MaxWrongPasswordAttempts)
                {
                    this.Framework.Security.SaveUserLock(userName, password, true);
                    this.Context.WrongPasswordProcessCount = 0;

                    throw new WrongPasswordException(this.Context.FrameworkContext, "Login", this.Context.FrameworkContext.SystemId, this.Context.FrameworkContext.Globalization.GetGlobalizationKeyValue(context.SystemId, Constants.Message.LockedAccount));
                }

                var remaining = this.Framework.Context.Security.MaxWrongPasswordAttempts - this.Context.WrongPasswordProcessCount;
                var customMessage = string.Format(this.Context.FrameworkContext.Globalization.GetGlobalizationKeyValue(this.Context.FrameworkContext.SystemId, Constants.ExceptionType.WrongPassword), this.Framework.Context.Security.MaxWrongPasswordAttempts, remaining);

                throw new WrongPasswordException(this.Context.FrameworkContext, "Login", this.Context.FrameworkContext.SystemId, customMessage);

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

        public class SuratRights
        {
            public Surat.Business.Security.SuratRight WebAuditor;
            public Surat.Business.Security.SuratRight WebAuditManagement;
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

