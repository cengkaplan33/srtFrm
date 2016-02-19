using Surat.Base.Log;
using Surat.Base.Model.Entities;
using Surat.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Base.Repositories
{
    public class PasswordChangeRepository : GenericRepository<PasswordChange>
    {
        #region Constructor

        public PasswordChangeRepository(LogContext context)
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
