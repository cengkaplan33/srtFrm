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
    public class CompanySiteDBPeriodsRepository : GenericRepository<CompanySiteDBPeriod>
    {
        #region Constructor

        public CompanySiteDBPeriodsRepository(SecurityContext context)
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
                    throw new NullValueException(this.Context.ApplicationContext,"CompanySiteDBPeriodsRepository.Context", this.Context.ApplicationContext.SystemId);
                return context;
            }
        }

        public FrameworkDbContext DBContext
        {
            get
            {
                return (FrameworkDbContext)(this.GenericDBContext);
            }
        }     

        #endregion

        #region Methods

        public List<DBConnectionView> GetCompanyDBConnections(int companySiteId)
        {
            List<DBConnectionView> dbConnections;

            dbConnections = (from companySiteDBPeriods in this.Context.ApplicationContext.DBContext.CompanySiteDBPeriods
                             where (companySiteDBPeriods.CompanySiteId == companySiteId && companySiteDBPeriods.IsActive == true)
                             select new DBConnectionView()
                       {
                           DBEnvironmentType = (DBEnvironmentType)companySiteDBPeriods.DBEnvironmentType,
                           DBKeyName = companySiteDBPeriods.DBKeyName,
                           DBConnectionString = companySiteDBPeriods.ConnectionString,
                           IsDefault = companySiteDBPeriods.IsDefault
                       }).ToList();

            return dbConnections;
        }

        #endregion
    }
}
