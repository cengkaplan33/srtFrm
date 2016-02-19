using Surat.Base.Globalization;
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
    public class GlobalizationKeyRepository : GenericRepository<GlobalizationKey>
    {
        #region Constructor

        public GlobalizationKeyRepository(GlobalizationContext context)
            : base(context.ApplicationContext.DBContext)
        {

        }

        #endregion

        #region Private Members

        #endregion

        #region Public Members

        #endregion

        #region Methods

        #endregion
    }
}
