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
    public class ExternalSystemsUsersRepository : GenericRepository<ExternalSystemsUser>
    {
        #region Constructor

        public ExternalSystemsUsersRepository(SecurityContext context)
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
                    throw new NullValueException(this.Context.ApplicationContext,"ExternalSystemsUsers.Context", this.Context.ApplicationContext.SystemId);
                return context;
            }
        }          

        #endregion

        #region Methods

        public List<ExternalSystemsUsersView> GetAllExternalSystemsUsers()
        {
            List<ExternalSystemsUsersView> externalSystemsUsersList = null;

            externalSystemsUsersList = (from externalSystemsUsers in this.Context.ApplicationContext.DBContext.ExternalSystemsUsers                               
                               where (externalSystemsUsers.IsActive == true)
                               select new ExternalSystemsUsersView()
                               {
                                   Id = externalSystemsUsers.Id,
                                    DelegateDBObjectId = externalSystemsUsers.DelegateDBObjectId,
                                    DelegateDBObjectType = (DelegateObjectType)externalSystemsUsers.DelegateDBObjectType,
                                    Password = externalSystemsUsers.Password,
                                    SystemId = externalSystemsUsers.SystemId,
                                    UserName = externalSystemsUsers.UserName,
                                    FirmaDonemTipi = (MasterDBFirmaDonemiTipi)externalSystemsUsers.FirmaDonemTipi,
                                    FirmaDonem = externalSystemsUsers.FirmaDonem,
                                    FirmaDonemId = externalSystemsUsers.FirmaDonemId,
                                    DatabaseName = externalSystemsUsers.DbName,
                                    VarsayilanMi = externalSystemsUsers.VarsayilanMi
                               }).ToList();

            return externalSystemsUsersList;
        }

        #endregion
    }
}
