using KonsolideRapor.Base.Application;
using KonsolideRapor.Base.Model;
using KonsolideRapor.Business.Configuration;
using KonsolideRapor.Business.Manage;
using KonsolideRapor.Common.Application;
using Surat.Base.Cache;
using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace KonsolideRapor.Business.Application
{
    public class KonsolideRaporApplicationManager : ApplicationManager, IKonsolideRaporApplicationManager
    {
          #region Constructor

        public KonsolideRaporApplicationManager():this(null,null)
        {

        }

        public KonsolideRaporApplicationManager(FrameworkApplicationManager applicationManager, UserDetailedView currentUser)
        {
            if (applicationManager != null)
           
                this.framework = applicationManager;
                
            else this.framework = InitializeFramework();
            if (currentUser != null)
            {
                this.Context.CurrentUser = currentUser;
                InitializeUserRelatedContext();
            }
        }

        #endregion

        #region Private Members

        private KonsolideRaporApplicationContext context;
        private FrameworkApplicationManager framework;
        private KonsolideRaporManager konsolideRaporManager;
      
        private KonsolideRaporConfigurationManager configurationManager;        
 
        #endregion

        #region Public Members

        public FrameworkApplicationManager Framework
        {
            get
            {
                if (framework == null)
                    framework = InitializeFramework();

                return framework;
            }
        }

        public KonsolideRaporApplicationContext Context
        {
            get
            {
                if (context == null)
                    InitializeKonsolideRaporContext();

                return context;
            }
        }

        public KonsolideRaporManager KonsolideRaporManager
        {
            get
            {
                if (konsolideRaporManager == null)
                    InitializeKonsolideRaporManager();

                return konsolideRaporManager;
            }
        }

    

        public KonsolideRaporConfigurationManager Configuration
        {
            get
            {
                if (configurationManager == null)
                    InitializeConfigurationManager();

                return configurationManager;
            }
        }

      
        #endregion

        #region IKonsolideRaporApplicationManager

        public IApplicationContext GetKonsolideRaporApplicationContext()
        {
            return this.Context;
        }

        public IFrameworkManager GetFrameworkManager()
        {
            return this.Framework;
        }

        #endregion
        
        #region Methods

        private FrameworkApplicationManager InitializeFramework()
        {
            FrameworkApplicationManager framework;

            framework = new FrameworkApplicationManager();

            //if (currentUser != null)
            //    framework = new FrameworkApplicationManager(currentUser);
            //else framework = new FrameworkApplicationManager();

            return framework;
        }

        private void InitializeKonsolideRaporContext()
        {
            context = new KonsolideRaporApplicationContext(this);

            context.DBContext = new KonsolideRaporDbContext();

            this.Framework.Trace.AppendLine(this.Context.SystemName, "KonsolideRaporContext Initialized.", TraceLevel.Basic);
        }

        private void InitializeKonsolideRaporManager()
        {            
            konsolideRaporManager = new KonsolideRaporManager(this);
            this.Framework.Trace.AppendLine(this.Context.SystemName, "KonsolideRaporManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeConfigurationManager()
        {
            configurationManager = new KonsolideRaporConfigurationManager(this);
            this.Framework.Trace.AppendLine(this.Context.SystemName, "ConfigurationManager Initialized.", TraceLevel.Basic);
        }

        private void InitializeUserRelatedContext()
        {
            //Framework var olan bir kullanıcı ile başlatıldığında kullanılır. Kullanıcı oturum açmıştır. Aynı kullanıcı set edilerek, işlemlere devam edilir.
            //ToDo : Servis üzerinden gelindiği zaman bunlara gerek yok. Sayfa yükleme ve benzeri?? Ele alınmalıdır.

            if (this.Context.CurrentUser.IsAdmin)
            {
                if (!this.Framework.Context.Configuration.IsUserAccessiblePagesLoaded())
                {
                    List<AccessiblePageView> userAccessiblePages = (List<AccessiblePageView>)CacheUtility.GetCachedObject(Constants.CacheList.UserAccessiblePages);

                    if (userAccessiblePages == null)
                    {
                        userAccessiblePages = this.Framework.Configuration.Page.GetAllPages();
                        this.Framework.Cache.SetObjectInCache(Constants.CacheList.UserAccessiblePages, userAccessiblePages);
                    }

                    this.Framework.Context.Configuration.UserAccessiblePages = userAccessiblePages;
                }

                if (!this.Framework.Context.Configuration.IsUserAccessibleActionsLoaded())
                {
                    List<AccessibleActionView> userAccessibleActions = (List<AccessibleActionView>)CacheUtility.GetCachedObject(Constants.CacheList.UserAccessibleActions);

                    if (userAccessibleActions == null)
                    {
                        userAccessibleActions = this.Framework.Configuration.Action.GetAllActions();
                        this.Framework.Cache.SetObjectInCache(Constants.CacheList.UserAccessibleActions, userAccessibleActions);
                    }

                    this.Framework.Context.Configuration.UserAccessibleActions = userAccessibleActions;
                }
            }
            else
            {
                this.Framework.Context.Configuration.UserAccessiblePages = this.Framework.Security.GetUserAccessiblePages(this.Context.CurrentUser);
                this.Framework.Context.Configuration.UserAccessibleActions = this.Framework.Configuration.Action.GetAllActions();
            }
        }
     

        #endregion              

        #region Dispose
        
        public override void Dispose()
        {
            this.Framework.Trace.WriteTraceToFile();
            this.Context.Dispose();            
        }

        #endregion        
    }
}
