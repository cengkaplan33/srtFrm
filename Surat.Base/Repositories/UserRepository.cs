using Surat.Base.Exceptions;
using Surat.Base.Model.Entities;
using Surat.Base.Security;
using Surat.Common.Data;
using Surat.Common.ViewModel;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Repositories
{
    public class UserRepository : GenericRepository<SuratUser>
    {
        #region Constructor

        public UserRepository(SecurityContext context)
            : base(context.ApplicationContext.DBContext)
        {
            this.context = context;
        }

        #endregion

        #region Private Members

        public SecurityContext context;

        #endregion

        #region Public Members

        public SecurityContext Context
        {
            get { return context; }
        }

        #endregion

        #region Methods

        public SuratUser GetUser(int userId)
        {
            var user = this.GetObjectsByParameters(p => p.IsActive == true & p.Id == userId).FirstOrDefault();
            return user;
        }

        public int GetUserDefaultWorkgroup(int userId)
        {
            int? workgroupId;


            workgroupId = (from users in this.Context.ApplicationContext.DBContext.Users
                           where (users.Id == userId)
                           select users.DefaultWorkgroup).FirstOrDefault();

            if (!workgroupId.HasValue)
                throw new SuratBusinessException(this.Context.ApplicationContext, "GetUserDefaultWorkgroup", this.Context.ApplicationContext.SystemId, this.Context.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.Context.ApplicationContext.SystemId, Constants.Message.UserDefaultWorkgroupMissing));

            return workgroupId.Value;
        }

        public List<SuratUser> GetUsersByName(string nameStartsWith)
        {
            return this.GetObjectsByParameters(p => p.IsActive == true & p.Name.Contains(nameStartsWith)).ToList();
        }

        public List<SuratUser> GetUsersActive()
        {
            return this.GetObjectsByParameters(p => p.IsActive == true).ToList();
        }
        public SuratUser GetActiveDirectoryUser(string userName)
        {
            var activeDirectoryUser = this.GetObjectsByParameters(p => p.IsActive == true & p.IsActiveDirectoryUser == true & p.UserName == userName).FirstOrDefault();
            return activeDirectoryUser;
        }
        public bool IsAdmin(string userShortName, int userId)
        {
            bool resultByName, resultByRole;

            resultByName = (userShortName == Constants.Application.Admin);

            resultByRole = this.HasAdminRole(userId);

            return resultByName || resultByRole;
        }

        public bool HasAdminRole(int userId)
        {
            bool result = false;
            RelationGroup relationGroup;


            relationGroup = (from relationGroups in this.Context.ApplicationContext.DBContext.RelationGroups
                             join roles in this.Context.ApplicationContext.DBContext.Roles on relationGroups.RoleId equals roles.Id
                             where (roles.ObjectTypeName == Constants.Application.Admin)
                                   && ((relationGroups.RoleId != 0) && (relationGroups.UserId == userId) && (relationGroups.WorkgroupId == 0))
                                   && (relationGroups.IsActive == true)
                             select relationGroups).FirstOrDefault();

            if (relationGroup != null)
                result = true;

            return result;
        }

        public List<RoleShortView> GetUserRoles(int userId)
        {
            List<RoleShortView> roleList;

            roleList = (from relationGroups in this.Context.ApplicationContext.DBContext.RelationGroups
                        join roles in this.Context.ApplicationContext.DBContext.Roles on relationGroups.RoleId equals roles.Id
                        where ((relationGroups.RoleId != 0) && (relationGroups.UserId == userId) && (relationGroups.WorkgroupId == 0))
                              && (relationGroups.IsActive == true)
                        select new RoleShortView
                        {
                            Id = roles.Id,
                            Name = roles.Name
                        }).ToList();

            return roleList;
        }

        public List<CompanySiteView> GetUserCompanySites(int userId)
        {
            WorkgroupRepository workgroupRepository = new WorkgroupRepository(this.Context);
            List<WorkgroupView> userWorkgroupList;
            List<CompanySiteView> companySites = null;

            userWorkgroupList = (from relationGroups in this.Context.ApplicationContext.DBContext.RelationGroups
                                 join workgroups in this.Context.ApplicationContext.DBContext.Workgroups on relationGroups.WorkgroupId equals workgroups.Id
                                 where (relationGroups.UserId == userId) && (relationGroups.WorkgroupId != 0) && (relationGroups.RoleId == 0) && (relationGroups.IsActive == true)
                                 select new WorkgroupView()
                                 {
                                     CompanyId = workgroups.CompanyId,
                                     IsCompanySite = workgroups.isCompanySite,
                                     WorkgroupId = workgroups.Id,
                                     ParentWorkgroupId = workgroups.ParentId,
                                     WorkgroupName = workgroups.Name
                                 }).ToList();

            companySites = new List<CompanySiteView>();

            foreach (WorkgroupView workgroup in userWorkgroupList)
            {
                CompanySiteView companySite = ProcessWorkgroup(workgroup, this.Context.ActiveWorkgroups);

                if (companySite != null)
                    AddCompanySite(companySites, companySite);
            }

            return companySites;
        }

        private void AddCompanySite(List<CompanySiteView> companySites, CompanySiteView companySite)
        {
            CompanySiteShortView selectedCompanySite;
            CompanySiteRepository companySiteRepository = new CompanySiteRepository(this.Context);
            //Decorate with CompanyCode from CompanySite

            selectedCompanySite = companySiteRepository.GetCompanySiteByWorkgroupId(companySite.WorkgroupId);

            if (selectedCompanySite == null)
                throw new RecordNotFoundException(this.Context.ApplicationContext, "CompanySite", this.Context.ApplicationContext.SystemId,
                    string.Format(this.Context.ApplicationContext.Globalization.GetGlobalizationKeyValue(this.Context.ApplicationContext.SystemId, Constants.Message.WorkgroupCompanySiteNotFound), companySite.WorkgroupId));

            companySite.Id = selectedCompanySite.Id;
            companySite.CompanyCode = selectedCompanySite.CompanyCode;

            //Decorate with DBConnections from CompanySiteDBPeriods
            CompanySiteDBPeriodsRepository companySiteDBPeriodsRepository = new CompanySiteDBPeriodsRepository(this.Context);
            companySite.DBConnections = companySiteDBPeriodsRepository.GetCompanyDBConnections(selectedCompanySite.Id);

            companySite.SelectedDBConnection = companySite.DBConnections.Where(p => p.IsDefault == true).FirstOrDefault();

            companySites.Add(companySite);
        }

        private CompanySiteView ProcessWorkgroup(WorkgroupView workgroup, List<WorkgroupView> allWorkgroups)
        {
            CompanySiteView companySite = null;

            if (workgroup.IsCompanySite || workgroup.CompanyId == workgroup.WorkgroupId)
            {
                companySite = new CompanySiteView();
                companySite.WorkgroupId = workgroup.WorkgroupId;
                companySite.WorkgroupName = workgroup.WorkgroupName;
                return companySite;
            }

            if (workgroup.ParentWorkgroupId.HasValue)
            {
                WorkgroupView parentWorkgroup = allWorkgroups.Where(p => p.WorkgroupId == workgroup.ParentWorkgroupId.Value).FirstOrDefault();
                if (parentWorkgroup != null)
                    companySite = ProcessWorkgroup(parentWorkgroup, allWorkgroups);
            }

            return companySite;
        }

        #endregion
    }
}
