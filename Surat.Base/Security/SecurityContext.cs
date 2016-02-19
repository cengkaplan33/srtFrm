using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model;
using Surat.Common.Application;
using Surat.Common.Log;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Security
{
    public class SecurityContext : IDisposable
    {
        #region Constructor

        public SecurityContext(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private byte maxWrongPasswordAttempts;
        private byte minPasswordLength;        
        private byte cookieExpirationTimeAsDays;
        private byte maxPasswordChangePeriodAsDays;
        private bool isCaptchaActive;
        private bool isRememberMeActive;
        private byte webSessionTimeoutInMinutes;
        private byte wrongPasswordAttemptCount;
        private List<ExternalSystemsUsersView> externalSystemsUsers;
        private List<WorkgroupView> activeWorkgroups;

        #endregion

        #region Public Members       

        public FrameworkContext ApplicationContext
        {
            get 
            {
                if (applicationContext == null)
                    applicationContext = (FrameworkContext)this.FrameworkManager.GetApplicationContext();

                return applicationContext;
            }   
        }        

        public IFrameworkManager FrameworkManager
        {
            get
            {
                return frameworkManager;
            }
        }

        public ITraceManager Trace
        {
            get
            {
                return this.FrameworkManager.GetTraceManager();
            }
        }

        public byte MaxWrongPasswordAttempts
        {
            get { return maxWrongPasswordAttempts; }
            set { maxWrongPasswordAttempts = value; }
        }

        public byte MinPasswordLength
        {
            get { return minPasswordLength; }
            set { minPasswordLength = value; }
        }       

        public byte CookieExpirationTimeAsDays
        {
            get { return cookieExpirationTimeAsDays; }
            set { cookieExpirationTimeAsDays = value; }
        }

        public byte MaxPasswordChangePeriodAsDays
        {
            get { return maxPasswordChangePeriodAsDays; }
            set { maxPasswordChangePeriodAsDays = value; }
        }

        public bool IsCaptchaActive // To Do
        {
            get { return isCaptchaActive; }
            set { isCaptchaActive = value; }
        }

        public bool IsRememberMeActive // To Do
        {
            get { return isRememberMeActive; }
            set { isRememberMeActive = value; }
        } 

        public Byte WebSessionTimeoutInMinutes
        {
            get
            {                
                return webSessionTimeoutInMinutes;
            }
            set { webSessionTimeoutInMinutes = value; }
        }

        public byte WrongPasswordAttemptCount
        {
            get
            {
                return wrongPasswordAttemptCount;
            }
            set
            {
                wrongPasswordAttemptCount = value;
            }
        }

        public List<ExternalSystemsUsersView> ExternalSystemsUsers
        {
            get
            {
                if (externalSystemsUsers == null)
                    throw new NullValueException(this.ApplicationContext,"SecurityContext.ExternalSystemsUsers", this.ApplicationContext.SystemId);

                return externalSystemsUsers;
            }
            set
            {
                externalSystemsUsers = value;
            }
        }

        public List<WorkgroupView> ActiveWorkgroups
        {
            get
            {
                if (activeWorkgroups == null)
                    this.FrameworkManager.GetActiveWorkGroups();

                return activeWorkgroups;
            }
            set
            {
                activeWorkgroups = value;
            }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion

        #region Methods

        public bool IsExternalSystemUsersLoaded()
        {
            return externalSystemsUsers != null ? true : false;
        }

        #endregion

    }
}
