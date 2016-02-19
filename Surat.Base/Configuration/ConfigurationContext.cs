using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Base.Configuration
{
    public class ConfigurationContext : IDisposable
    {
        #region Constructor

        public ConfigurationContext(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private FrameworkProviderType frameworkProviderType;
        private List<SystemDetailedView> systems;
        private List<ParameterValueView> systemParameters;
        private List<AccessiblePageView> userAccessiblePages;
        private List<AccessibleActionView> userAccessibleActions;

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

        public FrameworkProviderType FrameworkProviderType
        {
            get
            {
                return frameworkProviderType;
            }
            set 
            {
                frameworkProviderType = value;
            }
        }

        public List<ParameterValueView> SystemParameters
        {
            get
            {
                if (systemParameters == null)
                    systemParameters = GetSystemParameters();

                return systemParameters;
            }
            set
            {
                systemParameters = value;
            }
        }

        public List<SystemDetailedView> Systems
        {
            get
            {
                if (systems == null)
                    systems = GetSystems();
                return systems;
            }
            set
            {
                systems = value;
            }
        }

        public List<AccessiblePageView> UserAccessiblePages
        {
            get
            {
                if (userAccessiblePages == null)
                    throw new NullValueException(this.ApplicationContext,"ConfigurationContext.UserAccessiblePages", this.ApplicationContext.SystemId);

                return userAccessiblePages;
            }
            set
            {
                userAccessiblePages = value;
            }
        }

        public List<AccessibleActionView> UserAccessibleActions
        {
            get
            {
                if (userAccessibleActions == null)
                    throw new NullValueException(this.ApplicationContext, "ApplicationContext.UserAccessibleActions", this.ApplicationContext.SystemId);

                return userAccessibleActions;
            }
            set
            {
                userAccessibleActions = value;
            }
        }        

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion

        #region Methods

        public bool IsUserAccessiblePagesLoaded()
        {
            return userAccessiblePages != null ? true : false; 
        }

        public bool IsUserAccessibleActionsLoaded()
        {
            return userAccessibleActions != null ? true : false;
        }

        private List<SystemDetailedView> GetSystems()
        {
            return this.FrameworkManager.GetSystems();
        }

        private List<ParameterValueView> GetSystemParameters()
        {
            return this.FrameworkManager.GetSystemParameters();
        }   

        public int GetSystemIdByName(string systemName)
        { 
            SystemDetailedView system;

            system = this.Systems.Where(p => p.ObjectTypeName == systemName).FirstOrDefault();
            if (system == null)
                throw new RecordNotFoundException(this.ApplicationContext, "GetSystemIdByName", this.ApplicationContext.SystemId,
                    string.Format(this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.ExceptionType.RecordNotFound), systemName));

            return system.Id;
        }

        #endregion
    }
}
