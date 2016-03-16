﻿using Surat.Base.Application;
using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Base.Providers;
using Surat.Base.Repositories;
using Surat.Base.Security;
using Surat.Business.Configuration;
using Surat.Business.Globalization;
using Surat.Business.Log;
using Surat.Common.Application;
using Surat.Common.Data;
using Surat.Common.Log;
using Surat.Common.Security;
using Surat.Common.Utilities;
using Surat.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Surat.Business.Security
{
    public class SecurityManager : ISecurityManager
    {
        #region Constructor

        public SecurityManager(IFrameworkManager frameworkManager)
        {
            this.frameworkManager = frameworkManager;  
        }

        #endregion

        #region Private Members

        private FrameworkContext applicationContext;
        private IFrameworkManager frameworkManager;
        private ITraceManager traceManager;
        private IAuthenticationProvider authenticationProvider;
        private UserSessionRepository userSession;
        private UserRepository user;
        private UserShortcutRepository userShortcut;
        private RoleRepository role;
        private WorkgroupRepository workgroup;
        private AccessibleItemRepository accessibleItem;  
        private FailedLoginRepository failedLogin;
        private RelationGroupRepository relationGroup;
        private ExternalSystemsUsersRepository externalSystemsUsers;
      
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

        public IAuthenticationProvider AuthenticationProvider
        {
            get
            {
                if (authenticationProvider == null)
                    authenticationProvider = InitializeAuthenticationProvider();
                return authenticationProvider;
            }
        }
        
        public SecurityContext Context
        {
            get
            {
                return this.ApplicationContext.Security;
            }
        }            

        #endregion

        #region Repositories

        public UserSessionRepository UserSession
        {
            get
            {
                if (userSession == null)
                    userSession = new UserSessionRepository(this.ApplicationContext.Log);

                return userSession;
            }
        }

        public UserShortcutRepository UserShortcut
        {
            get
            {
                if (userShortcut == null)
                    userShortcut = new UserShortcutRepository(this.ApplicationContext.Security);

                return userShortcut;
            }
        }

        public UserRepository User
        {
            get
            {
                if (user == null)
                    user = new UserRepository(this.ApplicationContext.Security);

                return user;
            }
        }

        public RoleRepository Role
        {
            get
            {
                if (role == null)
                    role = new RoleRepository(this.ApplicationContext.Security);

                return role;
            }
        }

        public WorkgroupRepository Workgroup
        {
            get
            {
                if (workgroup == null)
                    workgroup = new WorkgroupRepository(this.ApplicationContext.Security);

                return workgroup;
            }
        }

        public FailedLoginRepository FailedLogin
        {
            get
            {
                if (failedLogin == null)
                    failedLogin = new FailedLoginRepository(this.ApplicationContext.Security);

                return failedLogin;
            }
        }

        public AccessibleItemRepository AccessibleItem
        {
            get
            {
                if (accessibleItem == null)
                    accessibleItem = new AccessibleItemRepository(this.ApplicationContext.Security);

                return accessibleItem;
            }
        }

        public RelationGroupRepository RelationGroup
        {
            get
            {
                if (relationGroup == null)
                    relationGroup = new RelationGroupRepository(this.ApplicationContext.Security);

                return relationGroup;
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

        #region ISecurityManager

        public int GetUserDefaultWorkgroup(int userId)
        {
            return this.User.GetUserDefaultWorkgroup(userId);
        }

        public int? GetParentWorkgroupId(int workgroupId)
        {
            return this.Workgroup.GetParentWorkgroupId(workgroupId);
        }

        #endregion

        #region Methods

        #region Password

        public void PasswordQualityChecker(string password)
        {
            if (password.Length < this.Context.MinPasswordLength)
                throw new SecurityException(this.ApplicationContext, "PasswordQualityChecker", this.ApplicationContext.SystemId, 
                    string.Format(this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.Message.PasswordLenghtRule),this.Context.MinPasswordLength));

            if (password.IndexOfAny(Constants.Parameter.Digits.ToCharArray()) < 0)
                throw new SecurityException(this.ApplicationContext,"PasswordQualityChecker", this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.Message.PasswordAtLeastOneDigitRule));

            if (password.IndexOfAny(Constants.Parameter.UpperCharacters.ToCharArray()) < 0)
                throw new SecurityException(this.ApplicationContext, "PasswordQualityChecker", this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.Message.PasswordAtLeastOneUpperCharacterRule));

            if (password.IndexOfAny(Constants.Parameter.SpecialCharacters.ToCharArray()) < 0)
                throw new SecurityException(this.ApplicationContext, "PasswordQualityChecker - Special Characters : " + Constants.Parameter.SpecialCharacters, this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.Message.PasswordAtLeastOneSpecialCharacter));
        }
       
        #endregion

        #region Authentication

        private IAuthenticationProvider InitializeAuthenticationProvider()
        {
            IAuthenticationProvider  authenticationProvider = null;

            switch (this.ApplicationContext.Configuration.FrameworkProviderType)
            {
                case FrameworkProviderType.Surat:
                    {
                        authenticationProvider = new SuratAuthenticationProvider();
                    }
                    break;
                case FrameworkProviderType.Serendip:
                    {
                        authenticationProvider = new SerendipAuthenticationProvider();
                    }
                    break;
                default:
                    throw new InvalidTypeException(this.ApplicationContext,"AuthenticationProvider", this.ApplicationContext.SystemId,
                        string.Format(this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.ExceptionType.InvalidType), this.ApplicationContext.Configuration.FrameworkProviderType));
            }

            return authenticationProvider;
        }

        #endregion

        #region Authorization

        public bool HasActionRight(string actionTypeName)
        {
            if (Constants.Web.SharedActions.Any(p => p.Contains(actionTypeName)))
                return true;

            if (this.User.IsAdmin(this.ApplicationContext.CurrentUser.ShortName, this.ApplicationContext.CurrentUser.UserId))
                return true;            

            return this.ApplicationContext.Configuration.UserAccessibleActions.Any(p => p.TypeName == actionTypeName);          
        }

        public bool HasPageRight(string pageName)
        {
            if (this.User.IsAdmin(this.ApplicationContext.CurrentUser.ShortName, this.ApplicationContext.CurrentUser.UserId))
                return true;

            return this.ApplicationContext.Configuration.UserAccessiblePages.Any(p => (p.ObjectTypePrefix == pageName || p.PageName== pageName));
        }

        public bool HasRight(string actionName)
        {
            if (HasActionRight(actionName) || HasPageRight(actionName))
                return true;
            return false;
        }

        #endregion

        #region User

        public UserDetailedView ValidateUser(string userName, string password)
        {
            UserDetailedView currentUser = null;

            try
            {
                currentUser = this.AuthenticationProvider.ValidateUser(this.ApplicationContext, userName, password);
            }
            catch (WrongPasswordException exception)
            {
                this.SaveFailedLogin(userName, password);
                throw exception;
            }
            

            return currentUser;
        }        

        public List<AccessiblePageView> GetUserAccessiblePages(UserDetailedView currentUser)
        {
            List<AccessiblePageView> accessiblePages;

            string query = @"	
select DISTINCT suratPages.Id as PageId,suratPages.SystemId,suratPages.Name as PageName,suratPages.ObjectTypePrefix,suratPages.ObjectTypeName,
suratSystems.Name as SystemName,suratSystems.ParentId as SystemParentId,suratPages.SmallImagePath,suratPages.BigImagePath
from dbo.Pages suratPages
INNER JOIN SuratSystems suratSystems ON suratPages.SystemId = suratSystems.Id	
LEFT JOIN AccessibleItems accessibleItems ON suratPages.Id = accessibleItems.DBObjectId
where 
    suratPages.IsActive = 1
    AND 
	(
	suratPages.IsAccessControlRequired = 0
	OR
	(
	suratPages.IsAccessControlRequired = 1 and 
	(accessibleItems.DBObjectType = 1 AND accessibleItems.AccessRightTypeId = 1 AND accessibleItems.IsActive = 1) 
	AND
	(accessibleItems.RelationGroupId IN 
		(
		  Select relationGroups.Id from dbo.RelationGroups relationGroups
		  where (relationGroups.UserId = @UserId)
		  UNION

            /*** role den gelen erişim hakları ***/
		  Select relationGroups.Id from dbo.RelationGroups relationGroups
		  where 
		  (		  	
			 UserId = 0  AND  WorkgroupId = 0 AND
			 relationGroups.RoleId IN 
			(Select Distinct RoleId from dbo.RelationGroups relationGroups
			 where (relationGroups.UserId = @UserId AND RoleId != 0 AND WorkgroupId =0)
		     )
		  )
		  UNION
            /*** workgroup tan gelen erişim hakları ***/
		  Select relationGroups.Id from dbo.RelationGroups relationGroups
		  where 
		  (
			 UserId = 0  AND  RoleId = 0 AND
			 relationGroups.WorkgroupId IN 
			(Select Distinct WorkgroupId from dbo.RelationGroups relationGroups
			 where (relationGroups.UserId = @UserId AND RoleId = 0 AND WorkgroupId !=0)
			 )
		  )
		 )
	)))";
            accessiblePages = this.Context.ApplicationContext.DBContext.Database.SqlQuery<AccessiblePageView>(
                                query, new SqlParameter("@UserId", currentUser.UserId)).ToList(); 

            return accessiblePages;
        }

        public List<AccessibleActionView> GetUserAccessibleActions(UserDetailedView currentUser)
        {
            List<AccessibleActionView> accessibleActions;

            string query = @"	select DISTINCT suratFunctions.Id as ActionId,suratFunctions.ObjectTypeName	
	from dbo.Functions suratFunctions	
	LEFT JOIN AccessibleItems accessibleItems
	ON suratPages.Id = accessibleItems.DBObjectId
	where 
	(suratPages.IsAccessControlRequired = 0)
	OR
	(suratPages.IsActive = 1)
	AND
	(accessibleItems.DBObjectType = 5 AND accessibleItems.AccessRightTypeId = 1 AND accessibleItems.IsActive = 1) 
	AND
	(accessibleItems.RelationGroupId IN 
		(
		  Select relationGroups.Id from dbo.RelationGroups relationGroups
		  where (relationGroups.UserId = @UserId)
		  UNION
		  Select relationGroups.Id from dbo.RelationGroups relationGroups
		  where 
		  (		  	
			 UserId = 0  AND  WorkgroupId = 0 AND
			 relationGroups.RoleId IN 
			(Select Distinct RoleId from dbo.RelationGroups relationGroups
			 where (relationGroups.UserId = @UserId AND RoleId != 0 AND WorkgroupId =0)
		     )
		  )
		  UNION
		  Select relationGroups.Id from dbo.RelationGroups relationGroups
		  where 
		  (
			 UserId = 0  AND  RoleId = 0 AND
			 relationGroups.WorkgroupId IN 
			(Select Distinct WorkgroupId from dbo.RelationGroups relationGroups
			 where (relationGroups.UserId = @UserId AND RoleId = 0 AND WorkgroupId !=0)
			 )
		  )
		 )
	)";
            accessibleActions = this.Context.ApplicationContext.DBContext.Database.SqlQuery<AccessibleActionView>(
                                query, new SqlParameter("@UserId", currentUser.UserId)).ToList();

            return accessibleActions;
        }

        public List<RelationGroupAccessiblePageView> GetAccessiblePages(int userId, int roleId, int workgroupId)
        {
            int relationGroupId = this.RelationGroup.GetIdByParameters(userId, roleId, workgroupId);
            List<RelationGroupAccessiblePageView> accessiblePages;

            string query = @"select DISTINCT suratPages.Id as PageId,suratPages.SystemId,suratPages.Name as PageName,suratPages.ObjectTypeName,
	                            suratSystems.Name as SystemName,suratSystems.ParentId as SystemParentId
	                            ,case when exists (select 1 from accessibleItems a
	                            where a.DBObjectId=suratPages.Id and 
	                            a.DBObjectType = 1 AND a.AccessRightTypeId = 1 AND a.IsActive = 1 and a.RelationGroupId = @RelationGroupId) then 1 else 0 end as HasAccess	
	                            from dbo.Pages suratPages
	                            INNER JOIN SuratSystems suratSystems
	                            ON suratPages.SystemId = suratSystems.Id
	                            where 
	                            (suratPages.IsAccessControlRequired = 0)
	                            OR
	                            (suratPages.IsActive = 1)";
            accessiblePages = this.Context.ApplicationContext.DBContext.Database.SqlQuery<RelationGroupAccessiblePageView>(
                                query, new SqlParameter("@RelationGroupId", relationGroupId)).ToList();

            return accessiblePages;
        }

        public void SaveUserSession(UserDetailedView currentUser)
        {
            int initializedDBContextId;
            UserSession session = new UserSession();

            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                session.SessionStart = TimeUtility.GetCurrentDateTime();
                session.UserId = currentUser.UserId;

                this.UserSession.Add(session);               
                this.ApplicationContext.CommitDBChanges(initializedDBContextId);

                currentUser.SessionStart = session.SessionStart;
                currentUser.SessionId = session.Id;     
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.ApplicationContext, "SaveUserSession", this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.Message.FrameworkSessionNotStarted), exception);
            }
        }

        public void CloseUserSession()
        {
            int initializedDBContextId;
            UserSession session;

            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                session = this.UserSession.GetById(this.ApplicationContext.CurrentUser.SessionId);
                session.SessionEnd = TimeUtility.GetCurrentDateTime();
                this.UserSession.Add(session);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.ApplicationContext,"SaveUserSession", this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId,Constants.Message.FrameworkSessionNotStarted), exception);
            }
        }

        public void SaveUsers(IEnumerable<SuratUser> suratUser)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                foreach (SuratUser user in suratUser)
                {
                    this.SaveUser(user);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);

            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"SaveUsers", this.ApplicationContext.SystemId,exception);
            }
        }

        public void SaveUser(SuratUser user)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                
                if (user.Id == 0)
                {
                   
                    this.User.Add(user);
                }
                else
                {
                    SuratUser selectedUser = this.User.GetObjectByParameters(p => p.Id == user.Id);
                    if (selectedUser.Password != user.Password)
                    {
                        selectedUser.LastPasswordChangedDate = DateTime.Now;
                        selectedUser.Password = user.Password;
                    }

                    selectedUser.Name = user.Name;
                    selectedUser.Notes = user.Notes;
                    selectedUser.UserName = user.UserName;
                    selectedUser.ChangedDate = DateTime.Now;
                    selectedUser.IsActive = user.IsActive;
                    selectedUser.IsActiveDirectoryUser = user.IsActiveDirectoryUser;
                    selectedUser.IsExternalUser = user.IsExternalUser;
                    selectedUser.IsLocked = user.IsLocked;
                    this.User.Update(selectedUser);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"SaveUser", this.ApplicationContext.SystemId,exception);
            }           
        }

        public void DeleteUsers(IEnumerable<SuratUser> suratUser)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                foreach (SuratUser user in suratUser)
                {
                    this.DeleteUser(user);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);

            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeleteUsers", this.ApplicationContext.SystemId,exception);
            }
        }

        public void DeleteUser(SuratUser user)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                SuratUser selectedUser = this.User.GetObjectByParameters(p => p.Id == user.Id);
                selectedUser.IsActive = false;                

                this.User.Update(selectedUser);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeleteUser", this.ApplicationContext.SystemId,exception);
            }            
        }

        public void SaveUserLock(string userName, string password,bool isLocked)
        {
            try
            {
                this.AuthenticationProvider.LockUser(this.ApplicationContext, userName, password, isLocked);
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.ApplicationContext,"SaveUserLock", this.ApplicationContext.SystemId, exception);
            }
        }

        public string RemindUserPassword(int userId)
        {
            string password;
            try
            {
                password = this.AuthenticationProvider.GetUserPassword(this.ApplicationContext, userId);
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.ApplicationContext,"RemindUserPassword", this.ApplicationContext.SystemId, exception);
            }

            return password;
        }

        #endregion

        #region Role

        public void SaveRoles(IEnumerable<SuratRole> roles)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                foreach (SuratRole role in roles)
                {
                    this.SaveRole(role);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);

            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"SaveRoles", this.ApplicationContext.SystemId,exception);
            }
        }

        public void SaveRole(SuratRole role)
        {
            int initializedDBContextId;
            if (role.Id == 0)
            {
                try
                {
                    initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                    role.InsertedDate = DateTime.Now;
                    role.IsActive = true;
                    role.ChangedByUser = null;
                    role.ChangedDate = null;
                    this.Role.Add(role);
                }
                catch (Exception exception)
                {
                    throw new EntityProcessException(this.ApplicationContext,"SaveRole", this.ApplicationContext.SystemId,exception);
                }
            }
            else
            {
                try
                {

                    initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                    var roleOfDatabase = this.Role.GetObjectByParameters(m => m.Id == role.Id);
                    roleOfDatabase.ChangedByUser = 0;
                    roleOfDatabase.ChangedDate = DateTime.Now;
                    roleOfDatabase.InsertedByUser = 0;
                    roleOfDatabase.InsertedDate = DateTime.Now;
                    roleOfDatabase.IsActive = true;                    
                    roleOfDatabase.Name = role.Name;
                    roleOfDatabase.ObjectTypeName = role.ObjectTypeName;
                    this.Role.Update(roleOfDatabase);
                }
                catch (Exception exception)
                {

                    throw new EntityProcessException(this.ApplicationContext,"UpdateRole", this.ApplicationContext.SystemId,exception);
                }

            }
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);
        }

        public void DeleteRoles(IEnumerable<SuratRole> suratRoles)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                foreach (SuratRole role in suratRoles)
                {
                    this.DeleteRole(role);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);

            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeleteRoles", this.ApplicationContext.SystemId,exception);
            }
        }

        public void DeleteRole(SuratRole role)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                var roleOfDatabase = this.Role.GetObjectByParameters(m => m.Id == role.Id);
                roleOfDatabase.IsActive = false;
                roleOfDatabase.ChangedDate = DateTime.Now;
                this.Role.Update(roleOfDatabase);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeleteRole", this.ApplicationContext.SystemId, exception);
            }
            
        }

        #endregion

        #region RelationGroups

        public void SaveRelationGroup(RelationGroup relationGroup)
        {
            int initializedDBContextId;
            int? workgroupId = relationGroup.WorkgroupId;
            
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                if (workgroupId == 0)
                {
                    var relationGroupOfDatabase = this.RelationGroup.GetObjectByParameters(m => m.RoleId == relationGroup.RoleId & m.UserId == relationGroup.UserId);
                    if (relationGroupOfDatabase == null)
                    {
                        relationGroup.ChangedByUser = null;
                        relationGroup.ChangedDate = null;
                        relationGroup.InsertedByUser = 1;
                        relationGroup.InsertedDate = DateTime.Now;
                        relationGroup.IsActive = true;
                        this.RelationGroup.Add(relationGroup);
                    }
                    else
                    {
                        if (relationGroup.WorkgroupId == 0)
                        {
                            throw new Exception("Seçtiğiniz kullanıcıya daha önceden bu Rol tanımlaması yapılmış.");
                        }
                        else
                        {
                            throw new Exception("Seçtiğiniz kullanıcı zaten bu çalışma grubunda yer alıyor");
                        }
                    }
                }
                else
                {
                    var relationGroupOfDatabase = this.RelationGroup.GetObjectByParameters(m => m.WorkgroupId == relationGroup.WorkgroupId & m.UserId == relationGroup.UserId);
                    if (relationGroupOfDatabase == null)
                    {
                        relationGroup.ChangedByUser = null;
                        relationGroup.ChangedDate = null;
                        relationGroup.InsertedByUser = 1;
                        relationGroup.InsertedDate = DateTime.Now;
                        relationGroup.IsActive = true;
                        this.RelationGroup.Add(relationGroup);
                    }
                    else
                    {
                        if (relationGroup.WorkgroupId == 0)
                        {
                            throw new Exception("Seçtiğiniz kullanıcı zaten bu çalışma grubunda yer almaktadır.");
                        }
                        else
                        {
                            throw new Exception("Seçtiğiniz kullanıcı zaten bu çalışma grubunda yer alıyor");
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                
                throw new EntityProcessException(this.ApplicationContext,"SaveRelationGroup",this.ApplicationContext.SystemId,exception);
            }
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);
        }

        #endregion

        #region Workgroup      

        public void SaveWorkgroup(Workgroup workgroup)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                if (workgroup.Id == 0)
                {
                    this.Workgroup.Add(workgroup);
                }
                else
                {
                    Workgroup selectedWorkgroup = this.Workgroup.GetObjectByParameters(m => m.Id == workgroup.Id);

                    selectedWorkgroup.Name = workgroup.Name;
                    selectedWorkgroup.ObjectTypeName = workgroup.ObjectTypeName;
                    selectedWorkgroup.ParentId = workgroup.ParentId;

                    this.Workgroup.Update(selectedWorkgroup);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"SaveWorkgroup", this.ApplicationContext.SystemId,exception);
            } 
        }

        public void DeleteWorkgroups(IEnumerable<Workgroup> workgroups)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                foreach (Workgroup workgroup in workgroups)
                {
                    this.DeleteWorkgroup(workgroup);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);

            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeleteWorkgroups", this.ApplicationContext.SystemId,exception);
            }
        }

        public void DeleteWorkgroup(Workgroup workgroup)
        {
            int initializedDBContextId;
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                Workgroup selectedWorkgroup = this.Workgroup.GetObjectByParameters(m => m.Id == workgroup.Id);
                selectedWorkgroup.IsActive = false;
                this.Workgroup.Update(selectedWorkgroup);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext,"DeleteWorkgroup", this.ApplicationContext.SystemId, exception);
            }

        }

        #endregion

        #region FailedLogin

        public void SaveFailedLogin(string userName,string password)
        {
            int initializedDBContextId;
            FailedLogin failedLogin = new FailedLogin();
            try
            {
                initializedDBContextId = this.ApplicationContext.InitializeDBContext();

                failedLogin.LoginDate = TimeUtility.GetCurrentDateTime();
                failedLogin.PasswordUsed = password;  //ToDo : şifreli saklanmalıdır.
                try
                {
                    failedLogin.UserId = this.User.GetObjectsByParameters(p => p.UserName == userName).First().Id;
                }
                catch
                {
                    //UserId Default = 0.
                }

                this.FailedLogin.Add(failedLogin);
                this.ApplicationContext.CommitDBChanges(initializedDBContextId);          
            }
            catch (Exception exception)
            {
                this.ApplicationContext.Trace.AppendLine(this.ApplicationContext.SystemId.ToString(), "Exception : " + exception.ToString(), TraceLevel.Basic);
            }
        }

        #endregion

        #endregion        
    }
}
