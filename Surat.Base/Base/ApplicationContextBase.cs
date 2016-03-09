using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.Entity;
using Surat.Common.Utilities;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;

namespace Surat.Base
{
    public abstract class ApplicationContextBase : IDisposable
    {

        #region Constructor

        public ApplicationContextBase(IFrameworkManager frameworkManager,string systemName)
        {
            this.frameworkManager = frameworkManager;  
            this.systemName = systemName;
            initializedContexts = new List<InitializedContextView>();
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        protected DbContext dbContext;
        private string systemName;
        private int systemId;        
        private UserDetailedView currentUser;
        private List<InitializedContextView> initializedContexts;
        private InitializedContextView activeInitializedContext;
        private string machineName;
        private string applicationBaseType;
        private object currentVariables;
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

        public int SystemId
        {
            get
            {
                if (systemId == 0)
                    systemId = GetSystemId();

                return systemId;
            }
            set { systemId = value; }
        }

        public string SystemName
        {
            get
            {
                return systemName;
            }    
        }
        public string MachineName
        {
            get
            {
                return System.Environment.MachineName;
            }
            set
            {
                machineName = value;
            }
        }
        public string ApplicationBaseType
        {
            get {
                return applicationBaseType;
            }
            set { 
                applicationBaseType = value;
            }
        }
        public object CurrentVariables
        {
            get {
                return currentVariables;
            }
            set {
                currentVariables = value;
            }
        }
        public DbContext DBContext
        {
            get
            {
                return dbContext;
            }
        }

        public IApplicationContext ContextOperations
        {
            get
            {
                if (this is IApplicationContext)
                    return (IApplicationContext)this;
                else throw new InterfaceNotImplementedException(this.ApplicationContext, "ApplicationContextBase-IApplicationContext", this.SystemId);
            }
        }

        public UserDetailedView CurrentUser
        {
            get
            {
                if (currentUser == null)
                    currentUser = this.ContextOperations.GetCurrentUser();

                 return currentUser;
            }
            set
            {
                currentUser = value;
            }
        }

        #endregion

        #region Methods

        private int GetSystemId()
        {
            int systemId = 0;

            if (this.FrameworkManager.IsApplicationContextInitialized())
                systemId = this.ApplicationContext.Configuration.GetSystemIdByName(this.SystemName);

            return systemId;
        }

        private int SaveChangesInternal(int initializedContextId)
        {
            int result = 0;

            IEnumerable<DbEntityEntry> entries = this.DBContext.ChangeTracker.Entries().
                Where(e => e.Entity is IAuditableEntity
                          && (e.State == EntityState.Added
                              || e.State == EntityState.Modified));

            if (entries.Count() > 0)
            {
                foreach (var entry in entries)
                {
                    IAuditableEntity auditableEntity = (IAuditableEntity)entry.Entity;

                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.InsertedByUser = this.CurrentUser.UserId;
                        auditableEntity.InsertedDate = TimeUtility.GetCurrentDateTime();
                        auditableEntity.IsActive = true;
                    }
                    else
                    {
                        auditableEntity.ChangedByUser = this.CurrentUser.UserId;
                        auditableEntity.ChangedDate = TimeUtility.GetCurrentDateTime();
                    }
                }                            
            }

            try
            {
                result = dbContext.SaveChanges();
            }
            catch (DbEntityValidationException exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"SaveChanges : " + ExceptionUtility.GetEntityValidationErrors(exception), this.SystemId, exception);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"SaveChanges", this.SystemId, exception);
            }   

            return result;
        }

        public virtual int CommitDBChanges(int initializeContextId)
        {
            int result = 0;

            if (!IsInitializeDBContextCalled(initializeContextId))
                throw new DBContextNotInitializedException(this.ApplicationContext,"SaveChangesInternal", this.SystemId);

            try
            {
                if (IsSaveChangesRequired(initializeContextId))
                {
                    result = SaveChangesInternal(initializeContextId);
                }                
            }
            catch (Exception exception)
            {
                this.Rollback();
                throw exception;
            }
            finally
            {
                DeleteInitializedContext(initializeContextId);
            }           

            return result;
        }

        private void Rollback()
        {
            //ToDo : Bir şey yapılması gereklimidir. Kontrol edilmelidir.
            //Context e null atanabilir.
        }       

        public virtual int InitializeDBContext()
        {
            //ToDo daha önceden çağrılmış ve commit yapılmamış/yapılamamış. durumu araştırılmalıdır. Rollback ?
            //To do : İşaret koyulmalı + Log alınmalı. Kim ne zaman, Neyi başlattı.
            return InitializeDBContext(ContextMode.ReuseActiveContext);
        }

        public virtual int InitializeDBContext(ContextMode contextMode)
        {
            //To do : İşaret koyulmalı + Log alınmalı. Kim ne zaman, Neyi başlattı. Nekadar sürdü.

            InitializedContextView newRequestedContext = new InitializedContextView();
            newRequestedContext.Id = initializedContexts.Count + 1;
            newRequestedContext.ContextMode = contextMode;

            if (activeInitializedContext != null)
            {
                newRequestedContext.ParentId = activeInitializedContext.Id;
                newRequestedContext.ContextMode = contextMode; 
            }
            else //Root
            {
                newRequestedContext.ContextMode = ContextMode.ReuseActiveContext; 
                newRequestedContext.ParentId = 0;
            }

            activeInitializedContext = newRequestedContext;
            initializedContexts.Add(newRequestedContext);
            return newRequestedContext.Id;
        }       

        private void DeleteInitializedContext(int contextId)
        {
            InitializedContextView context ;
            context = this.initializedContexts.SingleOrDefault(p => p.Id == contextId);
            if (context != null)
            {
                this.initializedContexts.Remove(context);
                //Set Active Context
                if (context.ParentId != 0)
                    this.activeInitializedContext = this.initializedContexts.SingleOrDefault(p => p.Id == context.ParentId);
                else this.activeInitializedContext = null;

            }
            else throw new ItemNotFoundException(this.ApplicationContext,"ApplicationContextBase.InitializedContext", this.SystemId);
        }

        #endregion

        #region Business Rules

        private bool IsSaveChangesRequired(int initializedContextId)
        {
            if (activeInitializedContext.ContextMode == ContextMode.ReuseActiveContext && activeInitializedContext.ParentId == 0)
                return true;
            else if (activeInitializedContext.ContextMode == ContextMode.StartWithNewContext)
                return true;
            else return false;
        }

        private bool IsInitializeDBContextCalled(int initializeContextId)
        {
            InitializedContextView context;

            context = this.initializedContexts.SingleOrDefault(p => p.Id == initializeContextId);
            if (context != null)
                return true;
            else return false;
        }

        #endregion  
      
        #region IDisposable

        public virtual void Dispose()
        {
            this.DBContext.Dispose();
        }

        #endregion
    }
}
