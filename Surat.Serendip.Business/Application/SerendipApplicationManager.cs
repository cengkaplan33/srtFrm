using Serendip.Entity.Sistem;
using Surat.Base.Application;
using Surat.Base.Cache;
using Surat.Base.Exceptions;
using Surat.Business.Application;
using Surat.Business.Base;
using Surat.Business.Log;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.SerendipApplication.Base;
using Surat.SerendipApplication.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Surat.SerendipApplication.Business
{
    public class SerendipApplicationManager : ApplicationManager
    {

        #region Constructor

        public SerendipApplicationManager(FrameworkApplicationManager applicationManager)
        {
            this.framework = applicationManager;
        }

        #endregion

        #region Private Members

        private SerendipApplicationContext context;
        private FrameworkApplicationManager framework;
 
        #endregion

        #region Public Members

        public SerendipApplicationContext Context
        {
            get
            {
                if (context == null)
                    InitializeSerendipApplicationContext();

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

        #endregion

        #region Methods        

        private void InitializeSerendipApplicationContext()
        {
            ExternalSystemsUsersView externalSystemUser = null;

            context = new SerendipApplicationContext(this.Framework.Context);

            if (!this.Framework.Context.Security.IsExternalSystemUsersLoaded())
                LoadExternalSystemsUsers();

            externalSystemUser = GetSerendipConnectionInfo();

            if (externalSystemUser == null)
            {
                this.Framework.Trace.AppendLine(this.Context.SystemName, "Serendip bağlantı bilgileri alınamadı.", TraceLevel.Basic);
                throw new SuratBusinessException(this.Framework.Context, "Serendip.ExternalSystemUser", this.Context.SystemId, this.Framework.Context.Globalization.GetGlobalizationKeyValue(this.Context.SystemId,SerendipConstants.Message.SerendipUserNotFound));
            }
            else
            {
                context.DBKeyName = this.Framework.Context.CurrentUser.SelectedCompanySite.SelectedDBConnection.DBKeyName;
                context.DBUserName = externalSystemUser.UserName;
                context.DBUserPassword = externalSystemUser.Password;
            }

            this.Framework.Trace.AppendLine(this.Context.SystemName, "SerendipApplicationContext Initialized.", TraceLevel.Basic);
        }

        private void LoadExternalSystemsUsers()
        {
            List<ExternalSystemsUsersView> externalSystemsUsers = (List<ExternalSystemsUsersView>)CacheUtility.GetCachedObject(Constants.CacheList.ExternalSystemsUsers);

            if (externalSystemsUsers == null)
            {
                externalSystemsUsers = this.Framework.Security.ExternalSystemsUsers.GetAllExternalSystemsUsers();
                this.Framework.Cache.SetObjectInCache(Constants.CacheList.ExternalSystemsUsers, externalSystemsUsers);
            }

            this.Framework.Context.Security.ExternalSystemsUsers = externalSystemsUsers;
        }

        private ExternalSystemsUsersView GetSerendipConnectionInfo()
        {
            ExternalSystemsUsersView externalSystemsUsersView = null;

            //User Delegate
            externalSystemsUsersView = GetUserDelegate();
            
            if (externalSystemsUsersView == null)
            {
                //Role Delegate
                externalSystemsUsersView = GetRoleDelegate();

                if (externalSystemsUsersView == null)
                {
                    //Company Site Delegate
                    externalSystemsUsersView = GetCompanySiteDelegate();
                }                
            }

            return externalSystemsUsersView;            
        }

        private ExternalSystemsUsersView GetUserDelegate()
        {
            return this.Framework.Context.Security.ExternalSystemsUsers.Where
               (p => p.DelegateDBObjectType == DelegateObjectType.User && 
                     p.DelegateDBObjectId == this.Framework.Context.CurrentUser.UserId &&
                     p.SystemId == this.Context.SystemId
               ).FirstOrDefault();
        }

        private ExternalSystemsUsersView GetRoleDelegate()
        {
            ExternalSystemsUsersView externalSystemUser = null;
            List<RoleShortView> currentUserRoles;

            currentUserRoles = this.Framework.Security.User.GetUserRoles(this.Framework.Context.CurrentUser.UserId);

            foreach (RoleShortView role in currentUserRoles)
            {
                externalSystemUser = this.Framework.Context.Security.ExternalSystemsUsers.Where
                   (p => p.DelegateDBObjectType == DelegateObjectType.Role &&
                         p.DelegateDBObjectId == role.Id &&
                         p.SystemId == this.Context.SystemId
                   ).FirstOrDefault();

                if (externalSystemUser != null)
                    break;
            }            

            return externalSystemUser;
        }

        private ExternalSystemsUsersView GetCompanySiteDelegate()
        {
            ExternalSystemsUsersView externalSystemUser = null;

            externalSystemUser = this.Framework.Context.Security.ExternalSystemsUsers.Where
               (p => p.DelegateDBObjectType == DelegateObjectType.CompanySite &&
                     p.DelegateDBObjectId == this.Framework.Context.CurrentUser.SelectedCompanySite.Id &&
                     p.SystemId == this.Context.SystemId
               ).FirstOrDefault();

            return externalSystemUser;
        }      

        public void Login()
        {
            //string dbName = "066_ZAMBAK_8Eylul2015_eKiyak";// ITINA25Subat2015"";
            //string userName = "s1nerj1.1ntegrat1on";
            //string password = "1FBDD789FE104A75A5A6F32FE107D65E";

            SerendipApplicationContext context = this.Context;
            try
            {
                Serendip.WinFormLib.Provider.StopServer();
                Serendip.WinFormLib.Provider.StartServerWeb(this.Context.DBKeyName, this.Context.DBUserName, this.Context.DBUserPassword);

                Serendip.Server.Provider.ServicesProviderService.AuthenticationService.Login(this.Context.DBUserName, this.Context.DBUserPassword, this.Context.DBKeyName);

                AnaKategori[] kategoriler = AnaKategori.ActiveRecord.FindAllBySql<AnaKategori>("Select * From AnaKategori", null);
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.Framework.Context, "SerendipLogin", this.Context.SystemId, this.Framework.Context.Globalization.GetGlobalizationKeyValue(this.Context.SystemId, SerendipConstants.Message.SerendipIntegrationFailed), exception);
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

