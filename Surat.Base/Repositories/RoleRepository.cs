using Surat.Base.Model.Entities;
using Surat.Base.Security;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Repositories
{
    public class RoleRepository : GenericRepository<SuratRole>
    {
        #region Constructor

        public RoleRepository(SecurityContext context)
            : base(context.ApplicationContext.DBContext)
        {

        }

        #endregion

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods

        public List<SuratRole> GetRolesByName(string nameStartsWith)
        {
            return this.GetObjectsByParameters(p => p.IsActive == true & p.Name.Contains(nameStartsWith)).ToList();
        }
        public List<SuratRole> GetRolesActive()
        {
            return this.GetObjectsByParameters(p => p.IsActive == true).ToList();
        }
        #endregion
    }
}
