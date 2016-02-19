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
    public class ParameterChangeRepository : GenericRepository<ParameterChange>
    {
        #region Constructor

        public ParameterChangeRepository(LogContext context)
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
