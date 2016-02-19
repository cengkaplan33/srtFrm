using Surat.Base.ActiveDirectory;
using Surat.Base.Cache;
using Surat.Base.Configuration;
using Surat.Base.Exceptions;
using Surat.Base.Globalization;
using Surat.Base.Log;
using Surat.Base.Mail;
using Surat.Base.Model;
using Surat.Base.Security;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.Log;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Application
{
    public class FrameworkContext : ApplicationContextBase,IApplicationContext
    {

        #region Constructor

        public FrameworkContext(IFrameworkManager frameworkManager)
            : base(frameworkManager,Constants.Application.FrameworkSystemName)
        {
            this.frameworkManager = frameworkManager;
        }

        #endregion

        #region Private Members

        private IFrameworkManager frameworkManager;
        private List<ParameterValueView> systemParameters;
        private UserDetailedView currentUser;
        private ConfigurationContext configuration;
        private LogContext log;
        private MailContext mail;
        private CacheContext cache;
        private SystemContext system;
        private ProductContext product;
        private GlobalizationContext globalization;
        private SecurityContext security;
        private ActiveDirectoryContext activeDirectory;
 
        #endregion

        #region Public Members

        public List<ParameterValueView> SystemParameters
        {
            get
            {
                return systemParameters;
            }
            set { systemParameters = value; }
        }

        public new IFrameworkManager FrameworkManager
        {
            get
            {
                return frameworkManager;
            }
            set { frameworkManager = value; }
        }

        public ITraceManager Trace
        {
            get
            {
                return this.FrameworkManager.GetTraceManager();
            }
        }

        public new UserDetailedView CurrentUser
        {
            get
            {
                if (currentUser == null)
                    throw new NullValueException(this,"ApplicationContext.GetCurrentUser", this.SystemId);
                else
                {
                    if (currentUser.SelectedCompanySite == null)
                        throw new SuratBusinessException(this, "CurrentUser", this.SystemId, this.Globalization.GetGlobalizationKeyValue(this.SystemId,Constants.Message.UserCompanyMissing));
                }

                return currentUser;
            }
            set
            {
                currentUser = value;
                ResetDependentContexts();
            }
        }

        public bool IsCurrentUserAssigned
        {
            get
            {
                return this.currentUser != null ? true : false;
            }
        }

        public new FrameworkDbContext DBContext
        {
            get
            {
                return (FrameworkDbContext)dbContext;
            }
            set { dbContext = value; }
        }

        public ConfigurationContext Configuration
        {
            get
            {
                if (configuration == null)
                    configuration = ApplicationContextFactory.GetNewConfigurationContext(this);

                return configuration;
            }
            set { configuration = value; }
        }

        public SecurityContext Security
        {
            get
            {
                if (security == null)
                    security = ApplicationContextFactory.GetNewSecurityContext(this);

                return security;
            }
            set { security = value; }
        }

        public LogContext Log
        {
            get
            {
                if (log == null)
                    log = ApplicationContextFactory.GetNewLogContext(this);

                return log;
            }
            set { log = value; }
        }

        public MailContext Mail
        {
            get
            {
                if (mail == null)
                    mail = ApplicationContextFactory.GetNewMailContext(this);

                return mail;
            }
            set { mail = value; }
        }

        public CacheContext Cache
        {
            get
            {
                if (cache == null)
                    cache = ApplicationContextFactory.GetNewCacheContext(this);

                return cache;
            }
            set { cache = value; }
        }

        public ProductContext Product
        {
            get
            {
                if (product == null)
                    product = ApplicationContextFactory.GetNewProductContext(this);

                return product;
            }
            set { product = value; }
        }

        public SystemContext System
        {
            get
            {
                if (system == null)
                    system = ApplicationContextFactory.GetNewSystemContext(this);

                return system;
            }
            set { system = value; }
        }

        public GlobalizationContext Globalization
        {
            get
            {
                if (globalization == null)
                    globalization = ApplicationContextFactory.GetNewGlobalizationContext(this);

                return globalization;
            }
            set { globalization = value; }
        }

        public ActiveDirectoryContext ActiveDirectory
        {
            get
            {
                if (activeDirectory == null)
                    activeDirectory = ApplicationContextFactory.GetNewActiveDirectoryContext(this);

                return activeDirectory;
            }
            set { activeDirectory = value; }
        }

        #endregion

        #region IApplicationContext

        public UserDetailedView GetCurrentUser()
        {
            return this.CurrentUser;
        }        

        #endregion

        #region Methods            
    
        public void ResetDependentContexts()
        {
            //Config değerleri tutuyorlar. Değerlerin yeniden yüklenmesi için, Reset edilirler. Ornek bir durum; kullanıcı login olmadan bazı değerler yüklenmiş olabilir.
            //Kullanıcı login olduktan sonra, kullanıcıya bağlı değer yükleme yapılır.
            this.activeDirectory = null;
            this.configuration = null;
            this.globalization = null;
            this.log = null;
            this.mail = null;
            this.product = null;
            this.security = null;
            this.system = null;
        }
   
        #endregion                
    }
}

