using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Common.Migrations
{
    public class MigrationUtility
    {
        public static bool CheckEntityByParameter<T>(DbContext context, Expression<Func<T, bool>> predicate) where T : class
        {
            return context.Set<T>().Where(predicate).FirstOrDefault() != null ? true:false;            
        }
    }
}
