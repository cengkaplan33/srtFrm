using Surat.Base.Configuration;
using Surat.Base.Exceptions;
using Surat.Base.Model;
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
    public class CompanySiteRepository : GenericRepository<CompanySite>
    {
        #region Constructor

        public CompanySiteRepository(SecurityContext context)
            : base(context.ApplicationContext.DBContext)
        {
            this.context = context;
        }

        #endregion

        #region Private Members

        private SecurityContext context;       

        #endregion

        #region Public Members

        public SecurityContext Context
        {
            get
            {
                if (context == null)
                    throw new NullValueException(this.Context.ApplicationContext,"CompanySiteRepository.Context", this.Context.ApplicationContext.SystemId);
                return context;
            }
        }          

        #endregion

        #region Methods

        public CompanySiteShortView GetCompanySiteByWorkgroupId(int workgroupId)
        {
            CompanySiteShortView selectedCompanySite;

            selectedCompanySite = (from companySite in this.Context.ApplicationContext.DBContext.CompanySites
                                   where (companySite.RelatedWorkgroupId == workgroupId)
                                   select new CompanySiteShortView()
                             {
                                 Id = companySite.Id,
                                 CompanyCode = companySite.CompanyCode
                             }).FirstOrDefault();

            return selectedCompanySite;
        }

        public List<CompanySiteView> GetAllCompanySites()
        {
            List<CompanySiteView> companySiteList = null;

            companySiteList = (from companySites in this.Context.ApplicationContext.DBContext.CompanySites
                               join workgroups in this.Context.ApplicationContext.DBContext.Workgroups on companySites.RelatedWorkgroupId equals workgroups.Id
                               where (companySites.IsActive == true)
                               select new CompanySiteView()
                               {
                                   Id = companySites.Id,
                                   WorkgroupId = workgroups.Id,
                                   WorkgroupName = workgroups.Name,
                                   CompanyCode = companySites.CompanyCode,
                                   DBConnections = (from companySiteDBPeriods in this.Context.ApplicationContext.DBContext.CompanySiteDBPeriods
                                                    where (companySiteDBPeriods.CompanySiteId == companySites.Id)
                                                    select new DBConnectionView()
                                                    {
                                                        IsDefault = companySiteDBPeriods.IsDefault,
                                                        DBConnectionString = companySiteDBPeriods.ConnectionString,
                                                        DBEnvironmentType = (DBEnvironmentType)companySiteDBPeriods.DBEnvironmentType,
                                                        DBKeyName = companySiteDBPeriods.DBKeyName
                                                    }
                                                   ).ToList()

                               }).ToList();

            return companySiteList;
        }

        #endregion
    }
}
