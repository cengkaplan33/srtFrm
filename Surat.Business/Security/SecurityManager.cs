using Surat.Base.Application;
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
        //private SuratRights suratRights;
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
                    string.Format(this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.Message.PasswordLenghtRule), this.Context.MinPasswordLength));

            if (password.IndexOfAny(Constants.Parameter.Digits.ToCharArray()) < 0)
                throw new SecurityException(this.ApplicationContext, "PasswordQualityChecker", this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.Message.PasswordAtLeastOneDigitRule));

            if (password.IndexOfAny(Constants.Parameter.UpperCharacters.ToCharArray()) < 0)
                throw new SecurityException(this.ApplicationContext, "PasswordQualityChecker", this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.Message.PasswordAtLeastOneUpperCharacterRule));

            if (password.IndexOfAny(Constants.Parameter.SpecialCharacters.ToCharArray()) < 0)
                throw new SecurityException(this.ApplicationContext, "PasswordQualityChecker - Special Characters : " + Constants.Parameter.SpecialCharacters, this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.Message.PasswordAtLeastOneSpecialCharacter));
        }

        #endregion

        #region Authentication

        private IAuthenticationProvider InitializeAuthenticationProvider()
        {
            IAuthenticationProvider authenticationProvider = null;

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
                    throw new InvalidTypeException(this.ApplicationContext, "AuthenticationProvider", this.ApplicationContext.SystemId,
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

            return this.ApplicationContext.Configuration.UserAccessiblePages.Any(p => (p.ObjectTypePrefix == pageName || p.PageName == pageName));
        }

        public bool HasOperationRight(int rightId)
        {
            if (this.User.IsAdmin(this.ApplicationContext.CurrentUser.ShortName, this.ApplicationContext.CurrentUser.UserId))
                return true;

            return this.ApplicationContext.Configuration.UserAccessibleRightIds.Contains(rightId);
        }

        public bool HasRight(string actionName)
        {
            if (HasPageRight(actionName) || HasActionRight(actionName))
                return true;
            return false;
        }

        public bool HasRight(string actionName, AccessibleItemDBObjectType AccessType)
        {
            if (AccessType == AccessibleItemDBObjectType.Right)
                return HasOperationRight(int.Parse(actionName));
            else if (AccessType == AccessibleItemDBObjectType.Page)
                return HasPageRight(actionName);
            else if (AccessType == AccessibleItemDBObjectType.Action)
                return HasActionRight(actionName);

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

        public List<AccessibleRolePageView> GetUserAccessibleRolePages(int? roleId)
        {
            List<AccessibleRolePageView> accessibleRolePages;

            string query = @"	

select Id,SystemId,Name,ObjectTypePrefix,ObjectTypeName,BigImagePath,SmallImagePath,IsAccess=1 from Pages where Id in(
select DISTINCT suratPages.Id 
from dbo.Pages suratPages
INNER JOIN SuratSystems suratSystems ON suratPages.SystemId = suratSystems.Id    
JOIN AccessibleItems accessibleItems ON  accessibleItems.DBObjectType = 1   and suratPages.Id = accessibleItems.DBObjectId
where 
    suratPages.IsActive = 1
    AND 
       (
       
       (
       suratPages.IsAccessControlRequired = 1 and 
       (accessibleItems.DBObjectType = 1 AND accessibleItems.AccessRightTypeId = 1 AND accessibleItems.IsActive = 1) 
       AND
       (accessibleItems.RelationGroupId IN 
             (
                 
            /*** role den gelen erişim hakları ***/
               Select relationGroups.Id from dbo.RelationGroups relationGroups
               where 
               (                 
                     UserId = 0  AND  WorkgroupId = 0 AND RoleId = @RoleId
                    
                  
               )
                
             )
       )))
	   ) 
and pages.IsAccessControlRequired=1


union

select Id,SystemId,Name,ObjectTypePrefix,ObjectTypeName,BigImagePath,SmallImagePath,IsAccess=0 from Pages where Id not in(
select DISTINCT suratPages.Id 
from dbo.Pages suratPages
INNER JOIN SuratSystems suratSystems ON suratPages.SystemId = suratSystems.Id    
JOIN AccessibleItems accessibleItems ON  accessibleItems.DBObjectType = 1   and suratPages.Id = accessibleItems.DBObjectId
where 
    suratPages.IsActive = 1
    AND 
       (
       
       (
       suratPages.IsAccessControlRequired = 1 and 
       (accessibleItems.DBObjectType = 1 AND accessibleItems.AccessRightTypeId = 1 AND accessibleItems.IsActive = 1) 
       AND
       (accessibleItems.RelationGroupId IN 
             (
                 
            /*** role den gelen erişim hakları ***/
               Select relationGroups.Id from dbo.RelationGroups relationGroups
               where 
               (                 
                     UserId = 0  AND  WorkgroupId = 0 AND RoleId =@RoleId
                    
                  
               )
                
             )
       )))
	   ) 
and pages.IsAccessControlRequired=1";
            accessibleRolePages = this.Context.ApplicationContext.DBContext.Database.SqlQuery<AccessibleRolePageView>(
                                query, new SqlParameter("@RoleId", roleId)).ToList();

            return accessibleRolePages;
        }

        public List<AccessibleActionView> GetUserAccessibleActions(UserDetailedView currentUser)
        {
            List<AccessibleActionView> accessibleActions;

            string query = @"
select DISTINCT suratActions.Id as ActionId, suratActions.TypeName as TypeName
from dbo.SuratActions suratActions
INNER JOIN AccessibleItems accessibleItems ON accessibleItems.DBObjectId  = suratActions.Id  and accessibleItems.DBObjectType = 2 
where  
suratActions .IsActive = 1 and accessibleItems .IsActive = 1
AND
accessibleItems.AccessRightTypeId = 1 
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

        public HashSet<int> GetUserRightIds(UserDetailedView currentUser)
        {
            return UserRightCache.GetUserRights(currentUser.UserId);
        }

        public List<AccessibleUserRoleView> GetUserRoles(int? userId)
        {
            List<AccessibleUserRoleView> accessibleUserRoles;

            string query = @"	
select Id,Name,ObjectTypeName,IsAccess=1 
from SuratRoles  suratRoles
where suratRoles.IsActive = 1 and suratRoles.Id in(
Select relationGroups.RoleId  from dbo.RelationGroups relationGroups
where ( UserId = @UserId  AND  WorkgroupId = 0 AND RoleId  != 0 and IsActive = 1 ))
union 
select Id,Name,ObjectTypeName,IsAccess=0 
from SuratRoles  suratRoles
where suratRoles.IsActive = 1 and suratRoles.Id not in(
Select relationGroups.RoleId  from dbo.RelationGroups relationGroups
where ( UserId = @UserId  AND  WorkgroupId = 0 AND RoleId  != 0 and IsActive = 1 ))

";
            accessibleUserRoles = this.Context.ApplicationContext.DBContext.Database.SqlQuery<AccessibleUserRoleView>(
                                query, new SqlParameter("@UserId", userId)).ToList();

            return accessibleUserRoles;
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
                throw new SuratBusinessException(this.ApplicationContext, "SaveUserSession", this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.Message.FrameworkSessionNotStarted), exception);
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
                this.UserSession.Update(session);

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.ApplicationContext, "SaveUserSession", this.ApplicationContext.SystemId, this.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.ApplicationContext.SystemId, Constants.Message.FrameworkSessionNotStarted), exception);
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
                throw new EntityProcessException(this.ApplicationContext, "SaveUsers", this.ApplicationContext.SystemId, exception);
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
                throw new EntityProcessException(this.ApplicationContext, "SaveUser", this.ApplicationContext.SystemId, exception);
            }
        }

        public void SaveUserRoles(int userId, IList<UserRoleView> userRoles)
        {
            try
            {
                
                //int relationGroupId = GetRoleRelationGroupId(roleId);
                List<RelationGroup> oldRecords = this.RelationGroup.GetObjectsByParameters(m => m.UserId == userId &  m.IsActive == true & m.WorkgroupId == 0 & m.RoleId != 0).ToList();
                foreach (var userRole in userRoles)
                {
                    var currentRole= oldRecords.Where(m => m.RoleId == userRole.Id).ToList();

                    if (currentRole.Count() > 1)
                        throw new EntityProcessException(this.ApplicationContext, "SaveRolePages", this.ApplicationContext.SystemId);

                    if (currentRole.Count() > 0)
                    {
                        int initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                        Surat.Base.Model.Entities.RelationGroup relation = currentRole[0];

                        if (userRole.IsAccess == false)
                        {
                            
                            relation.IsActive = false;
                            this.RelationGroup.Update(relation);
                        }
                        else
                        {
                            relation.IsActive = true;
                            this.RelationGroup.Update(relation);
                        }
                        this.ApplicationContext.CommitDBChanges(initializedDBContextId);
                    }
                    else
                    {
                        if (userRole.IsAccess == true)
                        {
                            int initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                            Surat.Base.Model.Entities.RelationGroup relation = new RelationGroup();
                            relation.UserId = userId;
                            relation.RoleId = userRole.Id;
                            relation.WorkgroupId = 0;
                            this.RelationGroup.Add(relation);
                            this.ApplicationContext.CommitDBChanges(initializedDBContextId);
                        }
                    }
                }
                
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "SaveRolePages", this.ApplicationContext.SystemId, exception);
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
                throw new EntityProcessException(this.ApplicationContext, "DeleteUsers", this.ApplicationContext.SystemId, exception);
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
                throw new EntityProcessException(this.ApplicationContext, "DeleteUser", this.ApplicationContext.SystemId, exception);
            }
        }

        public void SaveUserLock(string userName, string password, bool isLocked)
        {
            try
            {
                this.AuthenticationProvider.LockUser(this.ApplicationContext, userName, password, isLocked);
            }
            catch (Exception exception)
            {
                throw new SuratBusinessException(this.ApplicationContext, "SaveUserLock", this.ApplicationContext.SystemId, exception);
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
                throw new SuratBusinessException(this.ApplicationContext, "RemindUserPassword", this.ApplicationContext.SystemId, exception);
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
                throw new EntityProcessException(this.ApplicationContext, "SaveRoles", this.ApplicationContext.SystemId, exception);
            }
        }

        public void SaveRole(SuratRole role)
        {
            int initializedDBContextId;
            initializedDBContextId = this.ApplicationContext.InitializeDBContext();
            if (role.Id == 0)
            {
                try
                {
                     this.Role.Add(role);
                     this.ApplicationContext.CommitDBChanges(initializedDBContextId);
                    initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                    this.RelationGroup.Add(new RelationGroup() { 
                    UserId=0,
                    WorkgroupId=0,
                    RoleId=role.Id
                    });
                    this.ApplicationContext.CommitDBChanges(initializedDBContextId);
                   
                }
                catch (Exception exception)
                {
                    throw new EntityProcessException(this.ApplicationContext, "SaveRole", this.ApplicationContext.SystemId, exception);
                }
            }
            else
            {
                try
                {

                    
                    var roleOfDatabase = this.Role.GetObjectByParameters(m => m.Id == role.Id);                   
                    roleOfDatabase.Name = role.Name;
                    roleOfDatabase.ObjectTypeName = role.ObjectTypeName;
                    this.Role.Update(roleOfDatabase);
                    this.ApplicationContext.CommitDBChanges(initializedDBContextId);
                }
                catch (Exception exception)
                {

                    throw new EntityProcessException(this.ApplicationContext, "UpdateRole", this.ApplicationContext.SystemId, exception);
                }

            }
          
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
                throw new EntityProcessException(this.ApplicationContext, "DeleteRoles", this.ApplicationContext.SystemId, exception);
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

                List<RelationGroup> relationGroups = this.RelationGroup.GetObjectsByParameters(m => m.RoleId == role.Id).ToList();

                foreach (var relationGroup in relationGroups)
                {
                    relationGroup.IsActive = false;
                    this.RelationGroup.Update(relationGroup);
                }
                List<int> relationGroupIds = relationGroups.Select(x => x.Id).ToList();
                List<AccessibleItem> accessibleItems = this.AccessibleItem.GetObjectsByParameters(m => relationGroupIds.Contains(m.RelationGroupId)).ToList();

                foreach (var accessibleItem in accessibleItems)
                {
                    accessibleItem.IsActive = false;
                    this.AccessibleItem.Update(accessibleItem);
                }

                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "DeleteRole", this.ApplicationContext.SystemId, exception);
            }

        }

        public void SaveRolePages(int roleId, IList<RolePageView> rolePages)
        {
            try
            {
                int initializedDBContextId = this.ApplicationContext.InitializeDBContext();
                int relationGroupId = GetRoleRelationGroupId(roleId);
                List<AccessibleItem> oldRecords = this.AccessibleItem.GetObjectsByParameters(m => m.RelationGroupId == relationGroupId & m.DBObjectType == (int)AccessibleItemDBObjectType.Page&m.IsActive==true).ToList();
                foreach (var rolePage in rolePages)
                {
                    if (oldRecords.Where(m => m.DBObjectId == rolePage.Id).Count() > 0)
                    {

                        if (rolePage.IsAccess == false)
                        {
                            Surat.Base.Model.Entities.AccessibleItem accesibleItem = this.AccessibleItem.GetObjectsByParameters(m => m.RelationGroupId == relationGroupId & m.DBObjectId == rolePage.Id & m.IsActive == true).First();
                            accesibleItem.IsActive = false;
                            this.AccessibleItem.Update(accesibleItem);
                        }
                    }
                    else
                    {
                        if (rolePage.IsAccess == true)
                        {
                            Surat.Base.Model.Entities.AccessibleItem accesibleItem = new AccessibleItem();
                            accesibleItem.RelationGroupId = relationGroupId;
                            accesibleItem.AccessRightTypeId = 1;
                            accesibleItem.DBObjectId = rolePage.Id;
                            accesibleItem.DBObjectType = (int)AccessibleItemDBObjectType.Page;
                            accesibleItem.EndDate = DateTime.Now.AddYears(1);
                            accesibleItem.StartDate = DateTime.Now;
                            this.AccessibleItem.Add(accesibleItem);
                        }
                    }
                }
                this.ApplicationContext.CommitDBChanges(initializedDBContextId);
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "SaveRolePages", this.ApplicationContext.SystemId, exception);
            }

        }

        #endregion

        #region Right

        public SuratRight RegisterRight(String Name, String Description, int SystemId)
        {
            return new SuratRight(Name, Description, SystemId).Register();
        }

        public SuratRight RegisterRight(SuratRight Right)
        {
            return new SuratRight(Right._valueKey, Right._description, Right._systemId).Register();
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

                throw new EntityProcessException(this.ApplicationContext, "SaveRelationGroup", this.ApplicationContext.SystemId, exception);
            }
            this.ApplicationContext.CommitDBChanges(initializedDBContextId);
        }
        public int GetRoleRelationGroupId(int roleId)
        {
            return this.RelationGroup.GetIdByParameters(0, roleId, 0);
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
                throw new EntityProcessException(this.ApplicationContext, "SaveWorkgroup", this.ApplicationContext.SystemId, exception);
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
                throw new EntityProcessException(this.ApplicationContext, "DeleteWorkgroups", this.ApplicationContext.SystemId, exception);
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
                throw new EntityProcessException(this.ApplicationContext, "DeleteWorkgroup", this.ApplicationContext.SystemId, exception);
            }

        }

        #endregion

        #region FailedLogin

        public void SaveFailedLogin(string userName, string password)
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

        #region UserSession

        public List<UserSessionView> GetUserSessionsList()
        {
            try
            {
                List<UserSessionView> sessions = null;

                sessions = (from userSessions in this.Context.ApplicationContext.DBContext.UserSessions
                            join users in this.Context.ApplicationContext.DBContext.Users on userSessions.UserId equals users.Id
                            select new UserSessionView()
                              {
                                  Id = userSessions.Id,
                                  UserName = users.UserName,
                                  SessionStart = userSessions.SessionStart,
                                  SessionEnd = userSessions.SessionEnd,
                                  IP = userSessions.IP
                              }).ToList();

                return sessions;
            }
            catch (Exception exception)
            {
                throw new EntityProcessException(this.ApplicationContext, "GetUserSessionsList", this.ApplicationContext.SystemId, exception);
            }
        }

        #endregion

        #endregion
    }
}

