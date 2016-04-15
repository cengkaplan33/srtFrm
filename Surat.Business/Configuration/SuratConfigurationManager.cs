using Surat.Base.Application;
using Surat.Base.Configuration;
using Surat.Base.Exceptions;
using Surat.Base.Model;
using Surat.Base.Model.Entities;
using Surat.Base.Repositories;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.Log;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surat.Business.Configuration
{
    public class SuratConfigurationManager : IDisposable
    {
        //İlk aşamada, bazı nesneler daha ilk değerlerini almadan, sistem ile ilgili bilgiye ihtiyaç duyulduğu için, özel geliştirmeler yapıldı. (DBContext in geçirildiği contructor)
        #region Constructor

        public SuratConfigurationManager(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;
        }

        public SuratConfigurationManager(FrameworkDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private ITraceManager traceManager;
        private FrameworkDbContext dbContext;
        private PageRepository page;
        private ActionRepository action;
        private ParameterRepository parameter;
        private SystemRepository system;
        private ToolbarRepository toolbar;
        private ToolbarItemRepository toolbarItem;

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

        public FrameworkDbContext DBContext
        {
            get
            {
                if (dbContext == null)
                    if (this.ApplicationContext.DBContext == null)
                        throw new NullValueException(this.ApplicationContext,"SuratConfigurationManager.DBContext", 0); //DBContext olmadığı için, systemId de alınamamıştır.
                
                return dbContext;
            }
        }

        public ConfigurationContext Context
        {
            get
            {
                return this.ApplicationContext.Configuration;
            }
        }

        #endregion

        #region Repositories

        public PageRepository Page
        {
            get
            {
                if (page == null)
                    page = new PageRepository(this.ApplicationContext.Configuration);

                return page;
            }
        }

        public ActionRepository Action
        {
            get
            {
                if (action == null)
                    action = new ActionRepository(this.ApplicationContext.Security);

                return action;
            }
        }

        public ParameterRepository Parameter
        {
            get
            {
                if (parameter == null)
                    parameter = new ParameterRepository(this.ApplicationContext.Configuration);

                return parameter;
            }
        }

        public SystemRepository System
        {
            get
            {
                if (system == null)
                    system = new SystemRepository(this.ApplicationContext.Configuration);

                return system;
            }
        }

        public ToolbarRepository Toolbar
        {
            get
            {
                if (toolbar == null)
                    toolbar = new ToolbarRepository(this.ApplicationContext.Configuration);

                return toolbar;
            }
        }

        public ToolbarItemRepository ToolbarItem
        {
            get
            {
                if (toolbarItem == null)
                    toolbarItem = new ToolbarItemRepository(this.ApplicationContext.Configuration);

                return toolbarItem;
            }
        }


        #endregion

        #region Methods

        #region Parameter

        public int GetSystemIdBySystemName(string systemName)
        {
            int systemId = 0;

            using (SystemRepository systemRepository = new SystemRepository(this.DBContext))
            {
                try
                {
                    systemId = systemRepository.GetSystemIdByTypeName(systemName);
                }
                catch (Exception)
                {                    
                    throw;
                }
                
            }

            return systemId;
        }

        public List<ParameterValueView> GetSystemParameters(string systemName)
        {
            List<ParameterValueView> systemParameters = null;
            int systemId;

            //Sistemin ilk aşamasında kullanıldığı için, yeni repository üzerinden farklı constructor kullanılarak erişim sağlandı. Henüz bazı nesneler ilk değerlerini almadı.

            try
            {
                using (SystemRepository systemRepository = new SystemRepository(this.DBContext))
                {
                    systemId = systemRepository.GetSystemIdByTypeName(systemName);
                }

                using (ParameterRepository parameterRepository = new ParameterRepository(this.DBContext))
                {
                    systemParameters = parameterRepository.GetParametersBySystem(systemId);
                }
            }
            catch (Exception exception)
            {                
                throw exception; //ToDo: ele alınmalıdır.
            }


            return systemParameters;
        }
      

        public void SaveParameter(Parameter parameter)
        {
            int initializedDBContextId;
            if (parameter.Id == 0)
            {
                try
                {
                    initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                    parameter.InsertedDate = DateTime.Now;
                    parameter.IsActive = true;
                    parameter.ChangedByUser = null;
                    parameter.ChangedDate = null;
                    this.Parameter.Add(parameter);
                }
                catch (Exception exception)
                {
                    throw new EntityProcessException(this.ApplicationContext, "SaveParameter", this.ApplicationContext.SystemId,exception);
                }
            }
            else
            {
                try
                {

                    initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                    var parameterOfDatabase = this.Parameter.GetObjectByParameters(m => m.Id == parameter.Id);
                    parameterOfDatabase.ChangedByUser = 0;
                    parameterOfDatabase.ChangedDate = DateTime.Now;
                    parameterOfDatabase.InsertedByUser = 0;
                    parameterOfDatabase.InsertedDate = DateTime.Now;
                    parameterOfDatabase.IsActive = true;
                    parameterOfDatabase.DBObjectType = parameter.DBObjectType;
                    parameterOfDatabase.DBObjectId = parameter.DBObjectId;
                    parameterOfDatabase.TypeName = parameter.TypeName;
                    //parameterOfDatabase.ParameterValue = parameter.ParameterValue;
                    this.Parameter.Update(parameterOfDatabase);
                }
                catch (Exception exception)
                {

                    throw new EntityProcessException(this.ApplicationContext,"UpdateParameter", this.ApplicationContext.SystemId,exception);
                }
            }
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);
        }

        public void DeleteParameter(Parameter parameter)
        {
            int initializedDBContextId;
            
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                var parameterOfDatabase = this.Parameter.GetObjectByParameters(m => m.Id == parameter.Id);
                parameterOfDatabase.IsActive = false;
                parameterOfDatabase.ChangedDate = DateTime.Now;
                this.Parameter.Update(parameterOfDatabase);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeleteParameter", this.ApplicationContext.SystemId,exception);
            }
            
        }

        #endregion

        #region Page

        public List<PagesListView> GetActivePageList()
        {
            try
            {
                List<PagesListView> Pages;
                Pages = (from pages in this.Context.ApplicationContext.DBContext.Pages
                                   join systems in this.Context.ApplicationContext.DBContext.Systems on pages.SystemId equals systems.Id
                                   join InUser in this.Context.ApplicationContext.DBContext.Users on pages.InsertedByUser equals InUser.Id
                                   from ChUser in this.Context.ApplicationContext.DBContext.Users.Where(u => u.Id == pages.ChangedByUser).DefaultIfEmpty()
                                   where(pages.IsActive == true)
                         select new PagesListView
                                   {
                                       Id = pages.Id,
                                       SystemParentId = systems.ParentId,
                                       SystemName = systems.ObjectTypeName, 
                                       SystemId = pages.SystemId,
                                       PageName = pages.Name,
                                       ObjectTypePrefix = pages.ObjectTypePrefix,
                                       IsAccessControlRequired = pages.IsAccessControlRequired,
                                       IsVisibleInMenu = pages.IsVisibleInMenu,
                                       InsertedByUser = pages.InsertedByUser,
                                       InsertedByUserName = InUser.UserName,
                                       InsertedDate = pages.InsertedDate,
                                       ChangedByUser = pages.ChangedByUser,
                                       ChangedByUserName = ChUser.UserName,
                                       ChangedDate = pages.ChangedDate
                                   }).ToList();

                return Pages;
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "GetActivePageList", this.ApplicationContext.SystemId, exception);
            }
        }

        public void SavePage(Page page)
        {
            int initializedDBContextId;

            if (page.Id == 0)
            {
                try
                {
                    initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                    page.InsertedDate = DateTime.Now;
                    page.IsActive = true;
                    page.ChangedByUser = null;
                    page.ChangedDate = null;
                    this.Page.Add(page);
                }
                catch (Exception exception)
                {
                    throw new EntityProcessException(this.ApplicationContext,"SavePage", this.ApplicationContext.SystemId,exception);
                }
            }
            else
            {
                try
                {

                    initializedDBContextId= this.ApplicationContext.InitializeDBContext();
                    var pageOfDatabase = this.Page.GetObjectByParameters(m => m.Id == page.Id);
                    pageOfDatabase.ChangedByUser = 0;
                    pageOfDatabase.ChangedDate = DateTime.Now;
                    pageOfDatabase.InsertedByUser = 0;
                    pageOfDatabase.InsertedDate = DateTime.Now;
                    pageOfDatabase.IsActive = true;
                    pageOfDatabase.SystemId = page.SystemId;
                    pageOfDatabase.Name = page.Name;
                    pageOfDatabase.ObjectTypeName = page.ObjectTypeName;
                    pageOfDatabase.ObjectTypePrefix = page.ObjectTypePrefix;
                    pageOfDatabase.IsAccessControlRequired = page.IsAccessControlRequired;
                    pageOfDatabase.IsVisibleInMenu = page.IsVisibleInMenu;
                    pageOfDatabase.BigImagePath = page.BigImagePath;
                    page.SmallImagePath = page.SmallImagePath;
                    this.Page.Update(pageOfDatabase);
                }
                catch (Exception exception)
                {

                    throw new EntityProcessException(this.ApplicationContext,"UpdatePage", this.ApplicationContext.SystemId,exception);
                }
            }
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);
        }

        public void DeletePage(Page page)
        {
            int initializedDBContextId;
            
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                var pageOfDatabase = this.Page.GetObjectByParameters(m => m.Id == page.Id);
                pageOfDatabase.IsActive = false;
                pageOfDatabase.ChangedDate = DateTime.Now;
                this.Page.Update(pageOfDatabase);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeletePage", this.ApplicationContext.SystemId,exception);
            }
            
        }

        #endregion

        #region Action

        public List<ActionsListView> GetActiveActionList()
        {
            try
            {
                List<ActionsListView> Actions;
                Actions = (from actions in this.Context.ApplicationContext.DBContext.Actions
                           join systems in this.Context.ApplicationContext.DBContext.Systems on actions.SystemId equals systems.Id
                           join InUser in this.Context.ApplicationContext.DBContext.Users on actions.InsertedByUser equals InUser.Id
                           from ChUser in this.Context.ApplicationContext.DBContext.Users.Where(u => u.Id == actions.ChangedByUser).DefaultIfEmpty()
                           orderby (actions.Name)
                           select new ActionsListView
                         {
                             Id = actions.Id,
                             SystemParentId = systems.ParentId,
                             SystemName = systems.ObjectTypeName,
                             SystemId = actions.SystemId,
                             Name = actions.Name,
                             Description = actions.Description,
                             InsertedByUser = actions.InsertedByUser,
                             InsertedByUserName = InUser.UserName,
                             InsertedDate = actions.InsertedDate,
                             ChangedByUser = actions.ChangedByUser,
                             ChangedByUserName = ChUser.UserName,
                             ChangedDate = actions.ChangedDate
                         }).ToList();

                return Actions;
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "GetActiveActionList", this.ApplicationContext.SystemId, exception);
            }
        }

        #endregion

        #region System

        public void SaveSystem(SuratSystem system)
        {
            int initializedDBContextId;
            if (system.Id == 0)
            {
                try
                {
                    initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                    system.InsertedDate = DateTime.Now;
                    system.IsActive = true;
                    system.ChangedByUser = null;
                    system.ChangedDate = null;
                    this.System.Add(system);
                }
                catch (Exception exception)
                {
                    throw new EntityProcessException(this.ApplicationContext,"SaveSystem", this.ApplicationContext.SystemId,exception);
                }
            }
            else
            {
                try
                {

                    initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                    var systemOfDatabase = this.System.GetObjectByParameters(m => m.Id == system.Id);
                    systemOfDatabase.ChangedByUser = 0;
                    systemOfDatabase.ChangedDate = DateTime.Now;
                    systemOfDatabase.InsertedByUser = 0;
                    systemOfDatabase.InsertedDate = DateTime.Now;
                    systemOfDatabase.IsActive = true;
                    systemOfDatabase.ObjectTypeName = system.ObjectTypeName;
                    systemOfDatabase.ParentId = system.ParentId;
                    this.System.Update(systemOfDatabase);
                }
                catch (Exception exception)
                {

                    throw new EntityProcessException(this.ApplicationContext,"UpdateSystem", this.ApplicationContext.SystemId,exception);
                }

            }
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);
        }

        public void DeleteSystem(SuratSystem system)
        {
            int initializedDBContextId;
            
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                var systemOfDatabase = this.System.GetObjectByParameters(m => m.Id == system.Id);
                systemOfDatabase.IsActive = false;
                systemOfDatabase.ChangedDate = DateTime.Now;
                this.System.Update(systemOfDatabase);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeleteSystem", this.ApplicationContext.SystemId,exception);
            }            
        }

        #endregion

        #endregion

        #region IDisposable

        public void Dispose()
        {

        }

        #endregion        
    }
}
