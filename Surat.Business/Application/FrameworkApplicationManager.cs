using Surat.Base.Application;
using Surat.Base.Cache;
using Surat.Base.Exceptions;
using Surat.Base.Model;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.Business.ActiveDirectory;
using Surat.Business.Base;
using Surat.Business.Cache;
using Surat.Business.Configuration;
using Surat.Business.Globalization;
using Surat.Business.Log;
using Surat.Business.Mail;
using Surat.Business.Security;
using Surat.Common.Application;
using Surat.Common.Cache;
using Surat.Common.Data;
using Surat.Common.Globalization;
using Surat.Common.Log;
using Surat.Common.Mail;
using Surat.Common.Security;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Surat.Business.Application
{
    public class FrameworkApplicationManager : ApplicationManager, IFrameworkManager
    {

        #region Constructor

        public FrameworkApplicationManager():this(null)
        {
            
        }

        public FrameworkApplicationManager(UserDetailedView currentUser)
        {
            this.context = InitializeApplicationContext();

            if (currentUser != null)
            {
                this.Context.CurrentUser = currentUser;
                InitializeUserRelatedContext();
            }
        }

        #endregion

        #region Private Members

        private FrameworkContext context;
        private bool isContextInitialized;
        private SecurityManager securityManager;
        private ActiveDirectoryManager activeDirectoryManager;
        private SuratConfigurationManager configurationManager;
        private GlobalizationManager globalizationManager;
        private UserEventManager userEventManager;
        private ExceptionManager exceptionManager;
        private MailManager mailManager;
        private CacheManager cacheManager;
        private TraceManager traceManager;
 
        #endregion

        #region Public Members

        public FrameworkContext Context
        {
            get
            {
                return context;
            }
        }

        public bool IsContextInitialized
        {
            get
            {
                return isContextInitialized;
            }
        }

        public SecurityManager Security
        {
            get
            {
                if (securityManager == null)
                    InitializeSecurityManager();

                return securityManager;
            }
        }

        public ActiveDirectoryManager ActiveDirectory
        {
            get
            {
                if (activeDirectoryManager == null)
                    InitializeActiveDirectoryManager();

                return activeDirectoryManager;
            }
        }

        public SuratConfigurationManager Configuration
        {
            get
            {
                if (configurationManager == null)
                    InitializeConfigurationManager();

                return configurationManager;
            }
        }

        public GlobalizationManager Globalization
        {
            get
            {
                if (globalizationManager == null)
                    InitializeGlobalizationManager();

                return globalizationManager;
            }
        }

        public UserEventManager UserEvent
        {
            get
            {
                if (userEventManager == null)
                    InitializeUserEventManager();

                return userEventManager;
            }
        }

        public ExceptionManager Exception
        {
            get
            {
                if (exceptionManager == null)
                    InitializeExceptionManager();

                return exceptionManager;
            }
        }

        public MailManager Mail
        {
            get
            {
                if (mailManager == null)
                    InitializeMailManager();

                return mailManager;
            }
        }

        public CacheManager Cache
        {
            get
            {
                if (cacheManager == null)
                    InitializeCacheManager();

                return cacheManager;
            }
        }

        public TraceManager Trace
        {
            get
            {
                if (traceManager == null)
                    InitializeTraceManager();

                return traceManager;
            }
        }

        #endregion

        #region Methods

        private FrameworkContext InitializeApplicationContext()
        {
            FrameworkContext context = null;

            try
            {
                context = new FrameworkContext(this); 
                context.DBContext = new FrameworkDbContext();

                //DBContext oluşturulduktan sonra yapılmalıdır. Framework Konfigurasyon değerleri de hazırlanmalıdır. Diğer sınıflardan gelecek isteklere cevap veremez.
                context.SystemParameters = GetFrameworkParameters(context);
                context.SystemId = GetFrameworkSystemId(context);

                this.isContextInitialized = true;                               
            }
            catch (Exception exception)
            {
                throw new ContextInitializationException(this.Context,"Framework.ApplicationContext", context.SystemId,exception);
            }

            return context;
        }

        private List<ParameterValueView> GetFrameworkParameters(FrameworkContext context)
        {
            List<ParameterValueView> parameters;

            // Sistemin ilk aşaması olduğu için, Direk CacheUtility kullanıldı. CacheManager henüz başlatılmadı. Çünkü parametreler okunuyor.
            parameters = (List<ParameterValueView>)CacheUtility.GetCachedObject(Constants.CacheList.ParameterValueList);

            if (parameters == null)
            {
                //Konfigurasyon değerleri direkt bir erişime ile alınmalıdır. Sistem yapılandırılması tamamlanmadı.

                using (SuratConfigurationManager configurationManager = new SuratConfigurationManager(context.DBContext))
                {
                    parameters = configurationManager.GetSystemParameters(context.SystemName);
                }

                CacheUtility.SetObjectInCache(Constants.CacheList.ParameterValueList, parameters); 
            }

            return parameters;
        }

        private int GetFrameworkSystemId(FrameworkContext context)
        {
            int systemId = 0;
            try
            {
                //Konfigurasyon değerleri direkt bir erişime ile alınmalıdır. Sistem yapılandırılması tamamlanmadı.

                using (SuratConfigurationManager configurationManager = new SuratConfigurationManager(context.DBContext))
                {
                    systemId = configurationManager.GetSystemIdBySystemName(Constants.Application.FrameworkSystemName);
                }
            }
            catch (Exception exception)
            {
                throw new ContextInitializationException(this.Context,"GetFrameworkSystemParameters", 0, exception); //SystemId alınmadığında.
            }

            return systemId;
        }        

        private void InitializeUserRelatedContext()
        {
            //Framework var olan bir kullanıcı ile başlatıldığında kullanılır. Kullanıcı oturum açmıştır. Aynı kullanıcı set edilerek, işlemlere devam edilir.
            //ToDo : Servis üzerinden gelindiği zaman bunlara gerek yok. Sayfa yükleme ve benzeri?? Ele alınmalıdır.

            if (this.Context.CurrentUser.IsAdmin)
            {
                if (!this.Context.Configuration.IsUserAccessiblePagesLoaded())
                {
                    List<AccessiblePageView> userAccessiblePages = (List<AccessiblePageView>)CacheUtility.GetCachedObject(Constants.CacheList.UserAccessiblePages);

                    if (userAccessiblePages == null)
                    {
                        userAccessiblePages = this.Configuration.Page.GetAllPages();
                        this.Cache.SetObjectInCache(Constants.CacheList.UserAccessiblePages, userAccessiblePages);
                    }

                    this.Context.Configuration.UserAccessiblePages = userAccessiblePages;
                }

                if (!this.Context.Configuration.IsUserAccessibleActionsLoaded())
                {
                    List<AccessibleActionView> userAccessibleActions = (List<AccessibleActionView>)CacheUtility.GetCachedObject(Constants.CacheList.UserAccessibleActions);

                    if (userAccessibleActions == null)
                    {
                        userAccessibleActions = this.Configuration.Action.GetAllActions();
                        this.Cache.SetObjectInCache(Constants.CacheList.UserAccessibleActions, userAccessibleActions);
                    }

                    this.Context.Configuration.UserAccessibleActions = userAccessibleActions;
                }
            }
            else
            {
                this.Context.Configuration.UserAccessiblePages = this.Security.GetUserAccessiblePages(this.Context.CurrentUser);
                this.Context.Configuration.UserAccessibleActions = this.Security.GetUserAccessibleActions(this.Context.CurrentUser);  
                //this.Configuration.Action.GetAllActions();
            }
        }

        public void SetCurrentUser(UserDetailedView currentUser)
        {
            this.Context.CurrentUser = currentUser;
            InitializeUserRelatedContext();
        }

        public void StartUserSession(UserDetailedView currentUser)
        {
            UserSession session = new UserSession();

            try
            {
                this.Security.SaveUserSession(currentUser);
                this.Context.CurrentUser = currentUser;

                InitializeUserRelatedContext();
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.Context, "StartUserSession", this.Context.SystemId, this.Context.Globalization.GetGlobalizationKeyValue(this.Context.SystemId,Constants.Message.FrameworkSessionNotStarted), exception);
            }
        }

        public void CloseUserSession()
        {
            UserSession session = new UserSession();

            try
            {
                this.Security.CloseUserSession();
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.Context,"StartUserSession", this.Context.SystemId, this.Context.Globalization.GetGlobalizationKeyValue(this.Context.SystemId,Constants.Message.FrameworkSessionNotStarted), exception);
            }
        }

        private void InitializeConfigurationManager()
        {
            configurationManager = new SuratConfigurationManager(this);
            this.Trace.AppendLine(this.Context.SystemName, "ConfigurationManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeSecurityManager()
        {            
            securityManager = new SecurityManager(this);
            this.Trace.AppendLine(this.Context.SystemName, "SecurityManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeActiveDirectoryManager()
        {
            activeDirectoryManager = new ActiveDirectoryManager(this.Context, this.Configuration);
            this.Trace.AppendLine(this.Context.SystemName, "ActiveDirectoryManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeGlobalizationManager()
        {
            globalizationManager = new GlobalizationManager(this);
            this.Trace.AppendLine(this.Context.SystemName, "GlobalizationManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeUserEventManager()
        {            
            userEventManager = new UserEventManager(this);
        }

        private void InitializeExceptionManager()
        {
            exceptionManager = new ExceptionManager(this);
            this.Trace.AppendLine(this.Context.SystemName, "ExceptionManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeMailManager()
        {            
            mailManager = new MailManager(this);
            this.Trace.AppendLine(this.Context.SystemName, "MailManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeCacheManager()
        {
            cacheManager = new CacheManager(this);
            this.Trace.AppendLine(this.Context.SystemName, "CacheManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeTraceManager()
        {           
            traceManager = new TraceManager(this);
        }

        public void PublishException(Exception exception)
        {
            if (this.Context.IsCurrentUserAssigned)
                this.Exception.Publish(this.Context, exception, this.Context.CurrentUser);
            else this.Exception.Publish(this.Context, exception,null);
        }

        public string GetGlobalizationKeyValue(int systemId,string globalizationKey)
        {
            if (this.IsContextInitialized)
                return this.Globalization.GetGlobalizationKeyValue(systemId,globalizationKey);
            else return globalizationKey;
        }

        #endregion              

        #region Dispose
        
        public override void Dispose()
        {
            try
            {
                if (this.IsContextInitialized)
                    this.Trace.WriteTraceToFile();  
                //ToDo : Else Trace hazır değil. Başka bir yöntem bulunmalıdır.

                this.Context.Dispose(); 
            }
            catch (Exception exception)
            {                
                //ToDo: Trace yazılamıyor. Başka bir yöntem bulunmalıdır.
                //ToDo: Exception Handle
                throw exception;
            }            
        }

        #endregion

        #region IFrameworkManager

        public IApplicationContext GetApplicationContext()
        {
            return (IApplicationContext)this.Context;
        }

        public bool IsApplicationContextInitialized()
        {
            return this.IsContextInitialized;
        }

        public List<SystemDetailedView> GetSystems()
        {
            List<SystemDetailedView> systems = (List<SystemDetailedView>)this.Cache.GetCachedObject(Constants.CacheList.SystemList);
            if (systems == null)
            {
                this.Context.Configuration.Systems = this.Configuration.System.GetSystems();
                this.Cache.SetObjectInCache(Constants.CacheList.SystemList, this.Context.Configuration.Systems);
            }
            else this.Context.Configuration.Systems = systems;

            return this.Context.Configuration.Systems;
        }
       
        public List<ParameterValueView> GetSystemParameters()
        {            
            return this.Context.SystemParameters;
        }

        public List<WorkgroupView> GetActiveWorkGroups()
        {
            List<WorkgroupView> workgroups = (List<WorkgroupView>)CacheUtility.GetCachedObject(Constants.CacheList.WorkgroupList);
            if (workgroups == null)
            {
                using (WorkgroupRepository workgroupRepository = new WorkgroupRepository(this.Context.DBContext))
                {
                    this.Context.Security.ActiveWorkgroups = workgroupRepository.GetAllActiveWorkGroups();
                    CacheUtility.SetObjectInCache(Constants.CacheList.WorkgroupList, workgroups);
                }
            }
            else this.Context.Security.ActiveWorkgroups = workgroups;

            return this.Context.Security.ActiveWorkgroups;
        }

        public ITraceManager GetTraceManager()
        {
            return this.Trace;
        }

        public IGlobalizationManager GetGlobalizationManager()
        {
            return this.Globalization;
        }

        public ICacheManager GetCacheManager()
        {
            return this.Cache;
        }

        public IMailManager GetMailManager()
        {
            return this.Mail;
        }

        public ISecurityManager GetSecurityManager()
        {
            return this.Security;
        }

        #endregion            
    }
}

