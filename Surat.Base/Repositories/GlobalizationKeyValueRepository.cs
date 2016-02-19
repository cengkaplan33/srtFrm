using Surat.Base.Globalization;
using Surat.Base.Model.Entities;
using Surat.Base.Security;
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
    public class GlobalizationKeyValueRepository : GenericRepository<GlobalizationKeyValue>
    {
        #region Constructor

        public GlobalizationKeyValueRepository(GlobalizationContext context)
            : base(context.ApplicationContext.DBContext)
        {
            this.context = context;
        }

        #endregion

        #region Private Members

        private GlobalizationContext context;

        #endregion

        #region Public Members

        public GlobalizationContext Context 
        { 
            get 
            {
                return context;
            }
        }

        #endregion

        #region Methods

        public List<GlobalizationKeyView> GetKeyValueListByCulture(int systemId,byte cultureId)
        {
            List<GlobalizationKeyView> keyValueList;

            keyValueList = (from keyValues in this.Context.ApplicationContext.DBContext.GlobalizationKeyValues
                            join keys in this.Context.ApplicationContext.DBContext.GlobalizationKeys on keyValues.GlobalizationKeyId equals keys.Id
                            where (keys.SystemId == systemId && keyValues.CultureId == cultureId)
                            select new GlobalizationKeyView
                            {
                                SystemId = (short)keys.SystemId,
                                Key = keys.Name,
                                Value = keyValues.Content
                            }).ToList();

            return keyValueList;
        }

        #endregion
    }
}
