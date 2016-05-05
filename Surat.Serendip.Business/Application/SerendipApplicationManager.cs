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
using System.Data;
using System.Linq;
using System.Text;
using Surat.Base.Repositories;
using Surat.Base.Model.Entities;

namespace Surat.SerendipApplication.Business
{
    public class SerendipApplicationManager : ApplicationManager
    {

        #region Constructor

        public SerendipApplicationManager(FrameworkApplicationManager applicationManager)
        {
            this.framework = applicationManager;

            if (context == null && this.framework.Context != null && this.framework.Context.IsCurrentUserAssigned)
                InitializeSerendipApplicationContext();
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private SerendipApplicationContext context;
        private FrameworkApplicationManager framework;
        private List<Veritabani> masterDbVeritabanlari;
        private List<ExternalSystemsUsersView> kullaniciMasterDbVeritabanlari;
        private ExternalSystemsUsersRepository externalSystemsUsers;

        #endregion

        #region Public Members

        public FrameworkContext ApplicationContext
        {
            get
            {
                if (applicationContext == null)
                    applicationContext = (FrameworkContext)this.framework.GetApplicationContext();

                return applicationContext;
            }
        }

        public SerendipApplicationContext Context
        {
            get
            {
                if (context == null && this.Framework.Context != null && this.Framework.Context.IsCurrentUserAssigned)
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

        public List<ExternalSystemsUsersView> KullaniciMasterDbVeritabanlari
        {
            get
            {
                if (kullaniciMasterDbVeritabanlari == null)
                  kullaniciMasterDbVeritabanlari = LoadKullaniciMasterDbVeritabanlari(null);

                return kullaniciMasterDbVeritabanlari;
            }
        }

        public List<Veritabani> MasterDbVeritabanlari
        {
            get
            {
                if (masterDbVeritabanlari == null)
                    LoadMasterDbVeritabanlari();

                return masterDbVeritabanlari;
            }
        }

        public ExternalSystemsUsersRepository ExternalSystemsUsers
        {
            get
            {
                if (externalSystemsUsers == null)
                    externalSystemsUsers = new ExternalSystemsUsersRepository(this.ApplicationContext.Security);

                return externalSystemsUsers;
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
                throw new SuratBusinessException(this.Framework.Context, "Serendip.ExternalSystemUser", this.Context.SystemId, this.Framework.Context.Globalization.GetGlobalizationKeyValue(this.Context.SystemId, SerendipConstants.Message.SerendipUserNotFound));
            }
            else
            {
                //context.DBKeyName = this.Framework.Context.CurrentUser.SelectedCompanySite.SelectedDBConnection.DBKeyName;
                context.ExternalUser = externalSystemUser;

                context.DBKeyName = externalSystemUser.DatabaseName;
                context.FirmaDonem = externalSystemUser.FirmaDonem;
                context.DBUserName = externalSystemUser.UserName;
                context.DBUserPassword = externalSystemUser.Password;
            }

            //Veritabani[] dbList = Serendip.Common.ConfigurationHelper.SerendipMasterDBList;

            //var ss = KullaniciMasterDbVeritabanlari; 

            //Login();
            this.Framework.Trace.AppendLine(this.Context.SystemName, "SerendipApplicationContext Initialized.", TraceLevel.Basic);

            //var sss = MasterDbVeritabanlari;
        }

        private void LoadMasterDbVeritabanlari()
        {
            this.masterDbVeritabanlari = Serendip.Common.ConfigurationHelper.SerendipMasterDBList.ToList();
        }

        //private void LoadKullaniciMasterDbVeritabanlari()
        //{
        //    List<ExternalSystemsUsersView> externalSystemsUsersViews = null;

        //    externalSystemsUsersViews = GetUserDelegates(this.framework.Context.CurrentUser.UserId);

        //    if (externalSystemsUsersViews == null)
        //    {
        //        externalSystemsUsersViews = Serendip.Common.ConfigurationHelper.SerendipMasterDBList.Select(sList => new ExternalSystemsUsersView()
        //                  {
        //                      Id = null,
        //                      DelegateDBObjectId = this.framework.Context.CurrentUser.UserId,
        //                      DelegateDBObjectType = DelegateObjectType.User,
        //                      SystemId = this.Context.SystemId,
        //                      UserName = "",
        //                      Password = "",
        //                      FirmaDonemTipi = (int?)sList.FirmaDonemiTipi,
        //                      FirmaDonem = sList.Donem,
        //                      FirmaDonemId = sList.FirmaDonemiID,
        //                      DatabaseName = sList.Adi,
        //                      VarsayilanMi = false,
        //                  }).ToList();

        //        kullaniciMasterDbVeritabanlari = externalSystemsUsersViews;
        //    }
        //    else
        //    {
        //        var temp = from sList in Serendip.Common.ConfigurationHelper.SerendipMasterDBList.ToList()
        //                   join uList in externalSystemsUsersViews on sList.FirmaDonemiID equals uList.FirmaDonemId
        //                   into p_d
        //                   from j in p_d.DefaultIfEmpty()
        //                   //where kog.Id == request.AktifOgrenci.Id
        //                   //where (!(ko.IsDefault.Value == true || ko.Bolge_Id == 70) || !k.KitapTuru.Contains("zümre"))
        //                   //select new  ExternalSystemsUsersView {   });
        //                   select new ExternalSystemsUsersView()
        //                        {
        //                            Id = j == null ? null : j.Id,
        //                            DelegateDBObjectId = this.framework.Context.CurrentUser.UserId,
        //                            DelegateDBObjectType = DelegateObjectType.User,
        //                            SystemId = this.Context.SystemId,
        //                            UserName = j == null ? "" : j.UserName,
        //                            Password = j == null ? "" : j.Password,
        //                            FirmaDonemTipi = j == null ? (int?)sList.FirmaDonemiTipi : j.FirmaDonemTipi,
        //                            FirmaDonem = j == null ? sList.Donem : j.FirmaDonem,
        //                            FirmaDonemId = j == null ? sList.FirmaDonemiID : j.FirmaDonemId,
        //                            DatabaseName = j == null ? sList.Adi : j.DatabaseName,
        //                            VarsayilanMi = j == null ? false : j.VarsayilanMi,
        //                        };
        //        kullaniciMasterDbVeritabanlari = temp.ToList();
        //    }
        //    //var allList = Serendip.Common.ConfigurationHelper.SerendipMasterDBList.ToList().Join<.Where(x=> x.DonemAdi );
        //}

        public List<ExternalSystemsUsersView> LoadKullaniciMasterDbVeritabanlari(int? userId)
        {
            List<ExternalSystemsUsersView> externalSystemsUsersViews = null;

            if (userId == null)
                userId = this.framework.Context.CurrentUser.UserId;

            externalSystemsUsersViews = GetUserDelegates(userId);

            if (externalSystemsUsersViews == null)
            {
                externalSystemsUsersViews = Serendip.Common.ConfigurationHelper.SerendipMasterDBList.Select(sList => new ExternalSystemsUsersView()
                {
                    Id = null,
                    DelegateDBObjectId = (long)userId,
                    DelegateDBObjectType = DelegateObjectType.User,
                    SystemId = this.Context.SystemId,
                    SuratUserId = (int)userId,
                    UserName = "",
                    Password = "",
                    FirmaDonemTipi = (MasterDBFirmaDonemiTipi)sList.FirmaDonemiTipi,
                    FirmaDonem = sList.Donem,
                    FirmaDonemId = sList.FirmaDonemiID,
                    DatabaseName = sList.Adi,
                    VarsayilanMi = false,
                    FirmaDonemTipiName = sList.FirmaDonemiTipi.ToString(),
                }).ToList();

                return externalSystemsUsersViews;
            }
            else
            {
                var temp = from sList in Serendip.Common.ConfigurationHelper.SerendipMasterDBList.ToList()
                           join uList in externalSystemsUsersViews on sList.FirmaDonemiID equals uList.FirmaDonemId
                           into p_d
                           from j in p_d.DefaultIfEmpty()
                           //where kog.Id == request.AktifOgrenci.Id
                           //where (!(ko.IsDefault.Value == true || ko.Bolge_Id == 70) || !k.KitapTuru.Contains("zümre"))
                           //select new  ExternalSystemsUsersView {   });
                           select new ExternalSystemsUsersView()
                           {
                               Id = j == null ? null : j.Id,
                               DelegateDBObjectId = (long)userId,
                               DelegateDBObjectType = DelegateObjectType.User,
                               SystemId = this.Context.SystemId,
                               SuratUserId = (int)userId,
                               UserName = j == null ? "" : j.UserName,
                               Password = j == null ? "" : j.Password,
                               FirmaDonemTipi = j == null ? (MasterDBFirmaDonemiTipi)sList.FirmaDonemiTipi : j.FirmaDonemTipi,
                               FirmaDonem = j == null ? sList.Donem : j.FirmaDonem,
                               FirmaDonemId = j == null ? sList.FirmaDonemiID : j.FirmaDonemId,
                               DatabaseName = j == null ? sList.Adi : j.DatabaseName,
                               VarsayilanMi = j == null ? false : j.VarsayilanMi,
                               FirmaDonemTipiName = j == null ? sList.FirmaDonemiTipi.ToString() : j.FirmaDonemTipi.ToString(),
                           };
                return temp.ToList();
            }
            //var allList = Serendip.Common.ConfigurationHelper.SerendipMasterDBList.ToList().Join<.Where(x=> x.DonemAdi );
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
                     p.SystemId == this.Context.SystemId &&
                     p.VarsayilanMi == true
               ).FirstOrDefault();
        }

        private int? GetUserVarsayilanDelegate(int userId)
        {
            var Varsayilan = this.Framework.Context.Security.ExternalSystemsUsers.Where
               (p => p.DelegateDBObjectType == DelegateObjectType.User &&
                     p.DelegateDBObjectId == userId &&
                     p.VarsayilanMi == true
               ).FirstOrDefault();

            if (Varsayilan == null)
                return -1;
            else
                return Varsayilan.Id;
        }

        private List<ExternalSystemsUsersView> GetUserDelegates(int? userId)
        {
            return this.Framework.Context.Security.ExternalSystemsUsers.Where
                    (p => p.DelegateDBObjectType == DelegateObjectType.User &&
                     p.DelegateDBObjectId == userId &&
                     p.SystemId == this.Context.SystemId 
                     ).ToList();
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
                //Veritabani[] dbList = Serendip.Common.ConfigurationHelper.SerendipMasterDBList;

                //var ss = KullaniciMasterDbVeritabanlari; 

                //DataTable dt = SerendipHelper.GetMasterDb(Serendip.Common.KonfigurasyonTuru.Sistem);

                //AnaKategori[] kategoriler = AnaKategori.ActiveRecord.FindAllBySql<AnaKategori>("Select * From AnaKategori", null);
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.Framework.Context, "SerendipLogin", this.Context.SystemId, this.Framework.Context.Globalization.GetGlobalizationKeyValue(this.Context.SystemId, SerendipConstants.Message.SerendipIntegrationFailed), exception);
            }
        }

        public void SaveExternalUser(ExternalSystemsUsersView externalUser)
        {
            try
            {
                if (externalUser.Id != null)
                {
                    ExternalSystemsUser ExtUser = this.ExternalSystemsUsers.GetObjectByParameters(m => m.Id == (int)externalUser.Id);
                    UpdateExternalUser(externalUser, ExtUser);
                }
                else
                {
                    ExternalSystemsUser ExtUser = this.ExternalSystemsUsers.GetObjectByParameters(m => m.DelegateDBObjectType == (int)DelegateObjectType.User & m.DelegateDBObjectId == (int)externalUser.SuratUserId & m.FirmaDonemId == externalUser.FirmaDonemId);

                    if (ExtUser != null)
                    {
                        UpdateExternalUser(externalUser, ExtUser);
                    }

                    else
                    {
                        AddExternalUser(externalUser);
                    }
                }
                CacheUtility.RemoveCachedObject(Constants.CacheList.ExternalSystemsUsers);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "SaveExternalUser",  this.Context.SystemId, exception);
            }

        }

        public void UpdateExternalUser(ExternalSystemsUsersView externalUser, ExternalSystemsUser ExtUser)
        {
            try
            {
                int initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                ExtUser.IsActive = true;
                ExtUser.UserName = externalUser.UserName;
                ExtUser.Password = externalUser.Password;
                ExtUser.FirmaDonem = externalUser.FirmaDonem;
                ExtUser.FirmaDonemTipi = (Int16)externalUser.FirmaDonemTipi;
                ExtUser.DbName = externalUser.DatabaseName;
                if (externalUser.VarsayilanMi == true)
                {
                    var VarsayilanId = GetUserVarsayilanDelegate(externalUser.SuratUserId);

                    if (VarsayilanId != -1)
                    {
                        if (VarsayilanId != externalUser.Id)
                        {
                            ExternalSystemsUser VarExtUser = this.ExternalSystemsUsers.GetObjectByParameters(m => m.Id == (int)VarsayilanId);
                            VarExtUser.VarsayilanMi = false;
                            this.ExternalSystemsUsers.Update(VarExtUser);
                        }
                    }
                }

                ExtUser.VarsayilanMi = externalUser.VarsayilanMi;
                this.ExternalSystemsUsers.Update(ExtUser);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "UpdateExternalUser", this.Context.SystemId, exception);
            }
        }

        public void AddExternalUser(ExternalSystemsUsersView externalUser)
        {
            try
            {
                int initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                var ExtUser = new ExternalSystemsUser();

                ExtUser.DelegateDBObjectType = (int)DelegateObjectType.User;
                ExtUser.DelegateDBObjectId = externalUser.SuratUserId;
                ExtUser.SystemId = this.Context.SystemId;
                ExtUser.UserName = externalUser.UserName;
                ExtUser.Password = externalUser.Password;
                ExtUser.FirmaDonemId = (int)externalUser.FirmaDonemId;
                ExtUser.FirmaDonem = externalUser.FirmaDonem;
                ExtUser.FirmaDonemTipi = (Int16)externalUser.FirmaDonemTipi;
                ExtUser.DbName = externalUser.DatabaseName;
                if (externalUser.VarsayilanMi == true)
                {
                    var VarsayilanId = GetUserVarsayilanDelegate(externalUser.SuratUserId);

                    if (VarsayilanId != -1)
                    {
                        ExternalSystemsUser VarExtUser = this.ExternalSystemsUsers.GetObjectByParameters(m => m.Id == (int)VarsayilanId);
                        VarExtUser.VarsayilanMi = false;
                        this.ExternalSystemsUsers.Update(VarExtUser);
                    }
                }

                ExtUser.VarsayilanMi = externalUser.VarsayilanMi;
                this.ExternalSystemsUsers.Add(ExtUser);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);              
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "AddExternalUser", this.Context.SystemId, exception);
            }
        }

        public void DeleteExternalUser(ExternalSystemsUsersView externalUser)
        {
            try
            {
                if (externalUser.Id != null)
                {
                    int initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                    ExternalSystemsUser ExtUser = this.ExternalSystemsUsers.GetObjectByParameters(m => m.Id == (int)externalUser.Id);
                    ExtUser.IsActive = false;

                    this.ExternalSystemsUsers.Update(ExtUser);

                    this.ApplicationContext.CommitDBChanges(initializedDBContextId);

                    CacheUtility.RemoveCachedObject(Constants.CacheList.ExternalSystemsUsers);
                }
                else { ;}

            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "DeleteExternalUser", this.Context.SystemId, exception);
            }
        }

        #endregion

        #region Dispose

        public override void Dispose()
        {
            this.Framework.Trace.WriteTraceToFile();
            if (this.Context != null)
                this.Context.Dispose();
        }

        #endregion
    }
}

