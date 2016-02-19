using Surat.Base.Application;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.Log;
using Surat.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Business.Log
{
    public class UserEventManager
    {
        #region Constructor

        public UserEventManager(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;            
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private ITraceManager traceManager;
        private UserEventRepository userEventOperations;
        private DBRowStateChangeRepository dbRowStateChange;
        private ParameterChangeRepository parameterChange;
        private PasswordChangeRepository passwordChange;

        #endregion

        #region Public Members

        public IFrameworkManager FrameworkManager
        {
            get
            {
                return frameworkManager;
            }
        }

        public FrameworkContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (FrameworkContext)this.FrameworkManager.GetApplicationContext();

                return applicationContext;
            }
        }
        public ITraceManager Trace
        {
            get
            {
                if (traceManager == null)
                    traceManager = this.ApplicationContext.FrameworkManager.GetTraceManager();

                return traceManager;
            }
        }
       
        #endregion

        #region Repositories

        public UserEventRepository UserEventOperations
        {
            get
            {
                if (userEventOperations == null)
                    userEventOperations = new UserEventRepository(this.ApplicationContext.Log);

                return userEventOperations;
            }
        }

        public DBRowStateChangeRepository DBRowStateChange
        {
            get
            {
                if (dbRowStateChange == null)
                    dbRowStateChange = new DBRowStateChangeRepository(this.ApplicationContext.Log);

                return dbRowStateChange;
            }
        }

        public ParameterChangeRepository ParameterChange
        {
            get
            {
                if (parameterChange == null)
                    parameterChange = new ParameterChangeRepository(this.ApplicationContext.Log);

                return parameterChange;
            }
        }

        public PasswordChangeRepository PasswordChange
        {
            get
            {
                if (passwordChange == null)
                    passwordChange = new PasswordChangeRepository(this.ApplicationContext.Log);

                return passwordChange;
            }
        }

        #endregion

        #region Methods

        #region UserEvent

        public void SaveUserEvent(UserEventType userEventType, long dbObjectId)
        {
            int initializedDBContextId;
            UserEvent userEvent = new UserEvent();

            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                userEvent.UserId = this.ApplicationContext.CurrentUser.UserId;
                userEvent.EventDate = TimeUtility.GetCurrentDateTime();
                userEvent.EventTypeId = (byte)userEventType;
                userEvent.DBObjectId = dbObjectId;

                this.UserEventOperations.Add(userEvent);
                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                this.Trace.AppendLine(this.ApplicationContext.SystemName, "Exception : " + exception.ToString(), TraceLevel.Basic);
            }
        }

        #endregion

        #endregion

    }
}
